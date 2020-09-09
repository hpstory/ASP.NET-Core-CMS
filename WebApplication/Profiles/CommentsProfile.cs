using AutoMapper;
using WebApplication.Entities;
using WebApplication.Models.Comments;

namespace WebApplication.Profiles
{
    public class CommentsProfile : Profile
    {
        public CommentsProfile()
        {
            CreateMap<CommentsAddDto, Comments>();
            CreateMap<CommentsDto, Comments>()
                .ForMember(
                    dest => dest.PublishTime,
                    opt => opt.MapFrom(src => src.Date)
                );
            CreateMap<Comments, CommentsDto>()
                .ForMember(
                    dest => dest.Date,
                    opt => opt.MapFrom(src => src.PublishTime)
                )
                .ForMember(
                    dest => dest.ArticleId,
                    opt => opt.MapFrom(src => src.Articles.ID)
                )
                .ForMember(
                    dest => dest.Author,
                    opt => opt.MapFrom(src => src.User.NickName)
                )
                .ForMember(
                    dest => dest.Avatar,
                    opt => opt.MapFrom(src => src.User.Avatar)
                );
        }
    }
}
