using AutoMapper;
using CourseHeaven.Catalog.Api.Features.Categories.Dtos;

namespace CourseHeaven.Catalog.Api.Features.Categories;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}