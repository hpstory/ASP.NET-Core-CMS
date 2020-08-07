using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Infrastructure;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/banners")]
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

        [HttpGet(Name = nameof(GetBannersAsync))]
        public async Task<ActionResult<IEnumerable<BannersDto>>> GetBannersAsync()
        {
            var entities = await _repositoryWrapper.Banners.GetAllAsync();
            var returnDto = _mapper.Map<IEnumerable<BannersDto>>(entities);
            return Ok(returnDto);
        }

        [HttpPost]
        public async Task<ActionResult<Banners>> CreateBannerAsync(BannersAddDto banner)
        {
            var entity = _mapper.Map<Banners>(banner);
            _repositoryWrapper.Banners.Create(entity);
            await _repositoryWrapper.Banners.SaveAsync();

            var returnDto = _mapper.Map<BannersDto>(entity);

            return CreatedAtRoute(nameof(GetBannersAsync), new { bannersId = returnDto.ID }, returnDto);
        }

        [HttpDelete("{bannerId}")]
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
        [HttpPut("{bannerId}")]
        public async Task<ActionResult<BannersDto>> UpdateBannerAsync(BannersDto banner)
        {
            var entity = await _repositoryWrapper.Banners.GetByIdAsync(banner.ID);
            if (entity == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<Banners>(banner);
            _repositoryWrapper.Banners.Update(result);
            await _repositoryWrapper.Banners.SaveAsync();
            return NoContent();
        }
    }
}
