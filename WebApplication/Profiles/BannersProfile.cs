using AutoMapper;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Profiles
{
    public class BannersProfile : Profile
    {
        public BannersProfile()
        {
            CreateMap<BannersAddOrUpdateDto, Banners>();
            CreateMap<Banners, BannersDto>();
            CreateMap<BannersDto, Banners>();
        }
    }
}
