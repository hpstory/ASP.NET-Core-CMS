using AutoMapper;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Profiles
{
    public class BannersProfile : Profile
    {
        public BannersProfile()
        {
            CreateMap<BannersAddDto, Banners>();
            CreateMap<Banners, BannersDto>();
        }
    }
}
