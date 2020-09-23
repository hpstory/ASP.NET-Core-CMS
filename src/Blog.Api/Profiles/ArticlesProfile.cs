using AutoMapper;
using Blog.Api.Entities;
using Blog.Api.Models.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Profiles
{
    public class ArticlesProfile : Profile
    {
        public ArticlesProfile()
        {
            CreateMap<ArticlesAddOrUpdateDto, Articles>();
                //.ForPath(
                    //dest => dest.User.NickName,
                    //opt => opt.MapFrom(src => src.PublisherName)
                //);
            CreateMap<Articles, ArticlesDto>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                //.ForMember(
                //    dest => dest.PublisherName,
                //    opt => opt.MapFrom(src => src.User.NickName))
                .ForMember(
                    dest => dest.PublishDate,
                    opt => opt.MapFrom(src => src.PublishDate.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<ArticlesDto, Articles>();
        }
    }
}
