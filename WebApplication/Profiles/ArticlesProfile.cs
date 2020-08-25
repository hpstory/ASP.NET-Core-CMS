using AutoMapper;
using WebApplication.Entities;
using WebApplication.Models.Articles;

namespace WebApplication.Profiles
{
    public class ArticlesProfile : Profile
    {
        public ArticlesProfile()
        {
            CreateMap<ArticlesAddOrUpdateDto, Articles>();
            CreateMap<Articles, ArticlesDto>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(
                    dest => dest.PublisherName,
                    opt => opt.MapFrom(src => src.User.NickName))
                .ForMember(
                    dest => dest.PublishDate,
                    opt => opt.MapFrom(src => src.PublishDate.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<ArticlesDto, Articles>();
        }
    }
}
