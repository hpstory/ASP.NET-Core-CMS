using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Infrastructure;
using WebApplication.Models.Categories;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api")]

    public class CategoriesController : ControllerBase
    {
        private IRepositoryWrapper RepositoryWrapper { get; }
        private IMapper Mapper { get; }
        private readonly CMSDbContext _dbContext;
        public CategoriesController(
            IRepositoryWrapper repositoryWrapper, 
            IMapper mapper,
            CMSDbContext dbContext)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            _dbContext = dbContext;
        }
        [HttpCacheExpiration(NoStore = true)]
        [HttpGet("categories", Name = nameof(GetCategoriesAsync))]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesAsync()
        {
            var entities = await RepositoryWrapper.Categories.GetAllAsync();
            var returnDto = Mapper.Map<IEnumerable<CategoryDto>>(entities);
            return Ok(returnDto);
        }

        //[HttpPost("category")]
        //public async Task<ActionResult<Categories>> CreateCategoryAsync(CategoryAddOrUpdateDto category)
        //{
        //    var entity = _mapper.Map<Categories>(category);
        //    _repositoryWrapper.Categories.Create(entity);
        //    await _repositoryWrapper.Banners.SaveAsync();

        //    var returnDto = _mapper.Map<CategoryDto>(entity);

        //    return CreatedAtRoute(nameof(GetCategoriesAsync), new { bannersId = returnDto.ID }, returnDto);
        //}
        [HttpCacheExpiration]
        [HttpCacheValidation]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("category/{categoryId}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategoryAsync(int categoryId, CategoryAddOrUpdateDto category)
        {
            var entity = await RepositoryWrapper.Categories.GetByIdAsync(categoryId);
            if (entity == null)
            {
                return NotFound();
            }
            Mapper.Map(category, entity);
            RepositoryWrapper.Categories.Update(entity);
            await RepositoryWrapper.Categories.SaveAsync();
            return NoContent();
        }
    }
}
