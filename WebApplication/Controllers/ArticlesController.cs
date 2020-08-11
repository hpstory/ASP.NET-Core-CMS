using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication.Controllers.DtoParameters;
using WebApplication.Entities;
using WebApplication.Helpers;
using WebApplication.Infrastructure;
using WebApplication.Models.Articles;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api")]
    public class ArticlesController: ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper { get; }
        private IMapper _mapper { get; }
        private readonly CMSDbContext _dbContext;
        public ArticlesController(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            CMSDbContext dbContext
            )
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        [HttpGet("articles", Name = nameof(GetArticlesAsync))]
        public async Task<ActionResult<IEnumerable<ArticlesDto>>> GetArticlesAsync([FromQuery] ArticleResourceParameters parameters)
        {
            var articles = await _repositoryWrapper.Articles.GetAllAsync(parameters);
            var previousPageLink = articles.HasPrevious ?
                CreateArticlesResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = articles.HasNext ?
                CreateArticlesResourceUri(parameters, ResourceUriType.NextPage) : null;
            var paginationMetadata = new
            {
                totalCount = articles.TotalCount,
                pageSize = articles.PageSize,
                currentPage = articles.CurrentPage,
                totalPages = articles.TotalPages,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
            var returnDto = _mapper.Map<IEnumerable<ArticlesDto>>(articles);
            return Ok(returnDto);
        }

        [HttpGet("article")]
        public async Task<ActionResult<ArticlesDto>> GetArticleAsync(int articleId)
        {
            var entity = await _repositoryWrapper.Articles.GetByIdAsync(articleId);
            if (entity == null)
            {
                return NotFound();
            }
            var returnDto = _mapper.Map<ArticlesDto>(entity);
            return Ok(returnDto);
        }
        [HttpPost("article")]
        public async Task<ActionResult<Articles>> CreateArticleAsync (ArticlesAddOrUpdateDto article)
        {
            var entity = _mapper.Map<Articles>(article);
            _repositoryWrapper.Articles.Create(entity);
            await _repositoryWrapper.Articles.SaveAsync();

            var returnDto = _mapper.Map<ArticlesDto>(entity);

            return CreatedAtRoute(nameof(GetArticlesAsync), new { articleId = returnDto.ID }, returnDto);
        }
        [HttpDelete("article/{articleId}")]
        public async Task<IActionResult> DeleteArticleAsync (int articleId)
        {
            var entity = await _repositoryWrapper.Articles.GetByIdAsync(articleId);
            if (entity == null)
            {
                return NotFound();
            }
            _repositoryWrapper.Articles.Delete(entity);
            await _repositoryWrapper.Articles.SaveAsync();
            return NoContent();
        }
        [HttpPut("article/{articleId}")]
        public async Task<ActionResult<ArticlesDto>> UpdateArticleAsync(int articleId, ArticlesAddOrUpdateDto article)
        {
            var entity = await _repositoryWrapper.Articles.GetByIdAsync(articleId);
            if (entity == null)
            {
                var addArticle = _mapper.Map<Articles>(article);
                _repositoryWrapper.Articles.Create(addArticle);
                await _repositoryWrapper.Articles.SaveAsync();

                var returnDto = _mapper.Map<ArticlesDto>(addArticle);
                return CreatedAtRoute(nameof(GetArticlesAsync), new { articleId = returnDto.ID }, returnDto);
            }
            _mapper.Map(article, entity);
            _repositoryWrapper.Articles.Update(entity);
            await _repositoryWrapper.Articles.SaveAsync();
            return NoContent();
        }

        private string CreateArticlesResourceUri(ArticleResourceParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetArticlesAsync), new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        categoryId = parameters.CategoryId,
                        searchQuery = parameters.SearchQuery,
                        orderBy = parameters.OrderBy,
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetArticlesAsync), new 
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        categoryId = parameters.CategoryId,
                        searchQuery = parameters.SearchQuery,
                        orderBy = parameters.OrderBy,
                    });
                default:
                    return Url.Link(nameof(GetArticlesAsync), new 
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        categoryId = parameters.CategoryId,
                        searchQuery = parameters.SearchQuery,
                        orderBy = parameters.OrderBy,
                    });
            }
        }
    }
}
