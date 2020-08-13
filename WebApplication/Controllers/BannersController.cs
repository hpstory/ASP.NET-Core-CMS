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
    [Authorize]
    [ApiController]
    [Route("api")]
    public class BannersController : ControllerBase
    {
        public IRepositoryWrapper _repositoryWrapper { get; }
        public IMapper _mapper { get; }
        private readonly CMSDbContext _dbContext;

        public BannersController(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            CMSDbContext dbContext)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet("banners", Name = nameof(GetBannersAsync))]
        public async Task<ActionResult<IEnumerable<BannersDto>>> GetBannersAsync()
        {
            var entities = await _repositoryWrapper.Banners.GetAllAsync();
            var returnDto = _mapper.Map<IEnumerable<BannersDto>>(entities);
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
            var entity = _mapper.Map<Banners>(banner);
            _repositoryWrapper.Banners.Create(entity);
            await _repositoryWrapper.Banners.SaveAsync();

            var returnDto = _mapper.Map<BannersDto>(entity);

            return CreatedAtRoute(nameof(GetBannersAsync), new { bannersId = returnDto.ID }, returnDto);
        }

        [HttpDelete("banner/{bannerId}")]
        public async Task<IActionResult> DeleteBannerAsync(int bannerId)
        {
            var entity = await _repositoryWrapper.Banners.GetByIdAsync(bannerId);
            if(entity == null)
            {
                return NotFound();
            }
            _repositoryWrapper.Banners.Delete(entity);
            await _repositoryWrapper.Banners.SaveAsync();
            return NoContent();
        }
        [HttpPut("banner/{bannerId}")]
        public async Task<ActionResult<BannersDto>> UpdateBannerAsync(int bannerId, BannersAddOrUpdateDto banner)
        {
            var entity = await _repositoryWrapper.Banners.GetByIdAsync(bannerId);
            if (entity == null)
            {
                var addBanner = _mapper.Map<Banners>(banner);
                _repositoryWrapper.Banners.Create(addBanner);
                await _repositoryWrapper.Banners.SaveAsync();
                
                var returnDto = _mapper.Map<BannersDto>(addBanner);
                return CreatedAtRoute(nameof(GetBannersAsync), new { bannersId = returnDto.ID }, returnDto);
            }
            _mapper.Map(banner, entity);
            _repositoryWrapper.Banners.Update(entity);
            await _repositoryWrapper.Banners.SaveAsync();
            return NoContent();
        }
    }
}
