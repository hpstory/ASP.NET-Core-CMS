using AutoMapper;
using Blog.Api.Entities;
using Blog.Api.Entities.Enum;
using Blog.Api.Infrastructure.Repository;
using Blog.Api.Models.Articles;
using Blog.Api.Models.DtoParameters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api")]
    // [HttpCacheExpiration]
    public class ArticlesController : ControllerBase
    {
        private IRepositoryWrapper RepositoryWrapper { get; }
        private IMapper Mapper { get; }
        private readonly BlogDbContext _dbContext;
        private ILogger _logger;

        public ArticlesController(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            BlogDbContext dbContext,
            ILogger<ArticlesController> logger
            )
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }

        //[HttpCacheExpiration(NoStore = true)]
        [HttpGet("articles", Name = nameof(GetArticlesAsync))]
        public async Task<ActionResult<IEnumerable<ArticlesDto>>> GetArticlesAsync([FromQuery] ArticleResourceParameters parameters)
        {
            _logger.LogInformation($"[GetArticlesParameter]: {parameters}");
            var articles = await RepositoryWrapper.Articles.GetAllAsync(parameters);
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
            var returnDto = Mapper.Map<IEnumerable<ArticlesDto>>(articles);
            return Ok(returnDto);
        }
        [HttpGet("article/{articleId}")]
        public async Task<ActionResult<ArticlesDto>> GetArticleAsync(int articleId)
        {
            _logger.LogInformation($"[GetArticleId]: {articleId}");
            var entity = await RepositoryWrapper.Articles.GetArticleAsync(articleId);
            if (entity == null)
            {
                _logger.LogWarning($"[ArticleIdNotFound]: {articleId}");
                return NotFound();
            }
            var returnDto = Mapper.Map<ArticlesDto>(entity);
            return Ok(returnDto);
        }
        //[HttpCacheExpiration]
        //[HttpCacheValidation]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("article")]
        public async Task<ActionResult<Articles>> CreateArticleAsync(ArticlesAddOrUpdateDto article)
        {
            var entity = Mapper.Map<Articles>(article);
            //var user = await _userManager.FindByNameAsync(article.PublisherName);
            var category = RepositoryWrapper.Categories.GetByIdAsync(article.CategoryID);
            if (category.Result != null)
            {
                entity.Category = category.Result;
            }
            //entity.User = user;
            RepositoryWrapper.Articles.Create(entity);
            await RepositoryWrapper.Articles.SaveAsync();
            var returnDto = Mapper.Map<ArticlesDto>(entity);

            return CreatedAtRoute(nameof(GetArticlesAsync), new { articleId = returnDto.ID }, returnDto);
        }
        //[HttpCacheExpiration]
        //[HttpCacheValidation]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("article/{articleId}")]
        public async Task<IActionResult> DeleteArticleAsync(int articleId)
        {
            var entity = await RepositoryWrapper.Articles.GetByIdAsync(articleId);
            if (entity == null)
            {
                _logger.LogWarning($"[ArticleIdNotFound]: {articleId}");
                return NotFound();
            }
            RepositoryWrapper.Articles.Delete(entity);
            await RepositoryWrapper.Articles.SaveAsync();
            return NoContent();
        }
        [HttpPut("article/{articleId}")]
        public async Task<ActionResult<ArticlesDto>> UpdateArticleAsync(int articleId, ArticlesAddOrUpdateDto article)
        {
            var entity = await RepositoryWrapper.Articles.GetArticleAsync(articleId);
            if (entity == null)
            {
                _logger.LogWarning($"[ArticleIdNotFound]: {articleId}");
                var addArticle = Mapper.Map<Articles>(article);
                RepositoryWrapper.Articles.Create(addArticle);
                await RepositoryWrapper.Articles.SaveAsync();

                var returnDto = Mapper.Map<ArticlesDto>(addArticle);
                return CreatedAtRoute(nameof(GetArticlesAsync), new { articleId = returnDto.ID }, returnDto);
            }
            Mapper.Map(article, entity);
            RepositoryWrapper.Articles.Update(entity);
            await RepositoryWrapper.Articles.SaveAsync();
            return NoContent();
        }

        private string CreateArticlesResourceUri(ArticleResourceParameters parameters, ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link(nameof(GetArticlesAsync), new
                {
                    pageNumber = parameters.PageNumber - 1,
                    pageSize = parameters.PageSize,
                    parameters.CategoryName,
                    searchQuery = parameters.SearchQuery,
                    orderBy = parameters.OrderBy,
                }),
                ResourceUriType.NextPage => Url.Link(nameof(GetArticlesAsync), new
                {
                    pageNumber = parameters.PageNumber + 1,
                    pageSize = parameters.PageSize,
                    parameters.CategoryName,
                    searchQuery = parameters.SearchQuery,
                    orderBy = parameters.OrderBy,
                }),
                _ => Url.Link(nameof(GetArticlesAsync), new
                {
                    pageNumber = parameters.PageNumber,
                    pageSize = parameters.PageSize,
                    parameters.CategoryName,
                    searchQuery = parameters.SearchQuery,
                    orderBy = parameters.OrderBy,
                }),
            };
        }
    }
}
