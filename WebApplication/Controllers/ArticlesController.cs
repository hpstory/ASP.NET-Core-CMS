using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WebApplication.Entities;
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
        public async Task<ActionResult<IEnumerable<ArticlesDto>>> GetArticlesAsync()
        {
            var entities = await _repositoryWrapper.Articles.GetAllAsync();
            var returnDto = _mapper.Map<IEnumerable<ArticlesDto>>(entities);
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
    }
}
