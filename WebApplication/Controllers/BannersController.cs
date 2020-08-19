using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Infrastructure;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize("admin, superuser, staff")]
    [ApiController]
    [Route("api")]
    public class BannersController : ControllerBase
    {
        public IRepositoryWrapper RepositoryWrapper { get; }
        public IMapper Mapper { get; }
        private readonly CMSDbContext _dbContext;

        public BannersController(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            CMSDbContext dbContext)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet("banners", Name = nameof(GetBannersAsync))]
        public async Task<ActionResult<IEnumerable<BannersDto>>> GetBannersAsync()
        {
            var entities = await RepositoryWrapper.Banners.GetAllAsync();
            var returnDto = Mapper.Map<IEnumerable<BannersDto>>(entities);
            return Ok(returnDto);
        }

        //[HttpGet("banner/{bannerId}")]
        //public async Task<ActionResult<BannersDto>> GetBannerAsync(int bannerId)
        //{
        //    var entity = await _repositoryWrapper.Banners.GetByIdAsync(bannerId);
        //    if(entity == null)
        //    {
        //        return NotFound();
        //    }
        //    var returnDto = _mapper.Map<BannersDto>(entity);
        //    return Ok(returnDto);
        //}

        [HttpPost("banner")]
        public async Task<ActionResult<Banners>> CreateBannerAsync(BannersAddOrUpdateDto banner)
        {
            var entity = Mapper.Map<Banners>(banner);
            RepositoryWrapper.Banners.Create(entity);
            await RepositoryWrapper.Banners.SaveAsync();

            var returnDto = Mapper.Map<BannersDto>(entity);

            return CreatedAtRoute(nameof(GetBannersAsync), new { bannersId = returnDto.ID }, returnDto);
        }

        [HttpDelete("banner/{bannerId}")]
        public async Task<IActionResult> DeleteBannerAsync(int bannerId)
        {
            var entity = await RepositoryWrapper.Banners.GetByIdAsync(bannerId);
            if(entity == null)
            {
                return NotFound();
            }
            RepositoryWrapper.Banners.Delete(entity);
            await RepositoryWrapper.Banners.SaveAsync();
            return NoContent();
        }
        [HttpPut("banner/{bannerId}")]
        public async Task<ActionResult<BannersDto>> UpdateBannerAsync(int bannerId, BannersAddOrUpdateDto banner)
        {
            var entity = await RepositoryWrapper.Banners.GetByIdAsync(bannerId);
            if (entity == null)
            {
                var addBanner = Mapper.Map<Banners>(banner);
                RepositoryWrapper.Banners.Create(addBanner);
                await RepositoryWrapper.Banners.SaveAsync();
                
                var returnDto = Mapper.Map<BannersDto>(addBanner);
                return CreatedAtRoute(nameof(GetBannersAsync), new { bannersId = returnDto.ID }, returnDto);
            }
            Mapper.Map(banner, entity);
            RepositoryWrapper.Banners.Update(entity);
            await RepositoryWrapper.Banners.SaveAsync();
            return NoContent();
        }
    }
}
