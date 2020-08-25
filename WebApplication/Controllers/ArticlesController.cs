using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
    [HttpCacheExpiration]
    [HttpCacheValidation]
    public class ArticlesController: ControllerBase
    {
        private IRepositoryWrapper RepositoryWrapper { get; }
        private IMapper Mapper { get; }
        private readonly CMSDbContext _dbContext;
        private ILogger _logger;

        public ArticlesController(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            CMSDbContext dbContext,
            ILogger<ArticlesController> logger
            )
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }
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
            var entity = await RepositoryWrapper.Articles.GetByIdAsync(articleId);
            if (entity == null)
            {
                _logger.LogWarning($"[ArticleIdNotFound]: {articleId}");
                return NotFound();
            }
            var returnDto = Mapper.Map<ArticlesDto>(entity);
            return Ok(returnDto);
        }
        [HttpPost("article")]
        public async Task<ActionResult<Articles>> CreateArticleAsync (ArticlesAddOrUpdateDto article)
        {
            var entity = Mapper.Map<Articles>(article);
            RepositoryWrapper.Articles.Create(entity);
            await RepositoryWrapper.Articles.SaveAsync();

            var returnDto = Mapper.Map<ArticlesDto>(entity);

            return CreatedAtRoute(nameof(GetArticlesAsync), new { articleId = returnDto.ID }, returnDto);
        }
        [HttpDelete("article/{articleId}")]
        public async Task<IActionResult> DeleteArticleAsync (int articleId)
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
            var entity = await RepositoryWrapper.Articles.GetByIdAsync(articleId);
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
                    categoryId = parameters.CategoryId,
                    searchQuery = parameters.SearchQuery,
                    orderBy = parameters.OrderBy,
                }),
                ResourceUriType.NextPage => Url.Link(nameof(GetArticlesAsync), new
                {
                    pageNumber = parameters.PageNumber + 1,
                    pageSize = parameters.PageSize,
                    categoryId = parameters.CategoryId,
                    searchQuery = parameters.SearchQuery,
                    orderBy = parameters.OrderBy,
                }),
                _ => Url.Link(nameof(GetArticlesAsync), new
                {
                    pageNumber = parameters.PageNumber,
                    pageSize = parameters.PageSize,
                    categoryId = parameters.CategoryId,
                    searchQuery = parameters.SearchQuery,
                    orderBy = parameters.OrderBy,
                }),
            };
        }
    }
}
