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
                );
        }
    }
}
