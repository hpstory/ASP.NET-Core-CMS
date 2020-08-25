using AutoMapper;
using System.Linq;
using WebApplication.Entities;
using WebApplication.Models.Categories;

namespace WebApplication.Profiles
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<CategoryAddOrUpdateDto, Categories>();
            CreateMap<Categories, CategoryDto>()
                .ForMember(
                    dest => dest.NewsCount,
                    opt => opt.MapFrom(src => src.Articles.Select(a => new
                    {
                        NewsCount = a.CategoryID
                    }).Count())
                );
            CreateMap<CategoryDto, Categories>();
        }
    }
}
