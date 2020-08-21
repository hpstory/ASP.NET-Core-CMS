using AutoMapper;
using System;
using WebApplication.Entities.Identity.Entities;
using WebApplication.Models.User;

namespace WebApplication.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInfoDto>()
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => DateTime.Now.Year - src.BirthDate.Year)
                );
        }
    }
}
