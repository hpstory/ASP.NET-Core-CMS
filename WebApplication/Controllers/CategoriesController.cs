using AutoMapper;
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
        private IRepositoryWrapper _repositoryWrapper { get; }
        private IMapper _mapper { get; }
        private readonly CMSDbContext _dbContext;
        public CategoriesController(
            IRepositoryWrapper repositoryWrapper, 
            IMapper mapper,
            CMSDbContext dbContext)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet("categories", Name = nameof(GetCategoriesAsync))]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesAsync()
        {
            var entities = await _repositoryWrapper.Categories.GetAllAsync();
            var returnDto = _mapper.Map<IEnumerable<CategoryDto>>(entities);
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

        [HttpPut("category/{categoryId}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategoryAsync(int categoryId, CategoryAddOrUpdateDto category)
        {
            var entity = await _repositoryWrapper.Categories.GetByIdAsync(categoryId);
            if (entity == null)
            {
                return NotFound();
            }
            _mapper.Map(category, entity);
            _repositoryWrapper.Categories.Update(entity);
            await _repositoryWrapper.Categories.SaveAsync();
            return NoContent();
        }
    }
}
