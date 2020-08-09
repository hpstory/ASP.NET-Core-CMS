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
            CreateMap<Articles, ArticlesDto>();
            CreateMap<ArticlesDto, Articles>();
        }
    }
}
