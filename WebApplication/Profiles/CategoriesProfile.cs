using AutoMapper;
using WebApplication.Entities;
using WebApplication.Models.Categories;

namespace WebApplication.Profiles
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<CategoryAddOrUpdateDto, Categories>();
            CreateMap<Categories, CategoryDto>();
            CreateMap<CategoryDto, Categories>();
        }
    }
}
