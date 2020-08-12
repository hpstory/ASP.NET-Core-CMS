using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    dest => dest.ArticleDate,
                    opt => opt.MapFrom(src => src.PublishDate)); ;
            CreateMap<ArticlesDto, Articles>()
                .ForMember(
                    dest => dest.PublishDate,
                    opt => opt.MapFrom(src => src.ArticleDate)); ;
        }
    }
}
