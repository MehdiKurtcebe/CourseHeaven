using AutoMapper;
using CourseHeaven.Basket.Api.Data;
using CourseHeaven.Basket.Api.Dtos;

namespace CourseHeaven.Basket.Api.Features.Baskets;

public class BasketMapping : Profile
{
    public BasketMapping()
    {
        CreateMap<BasketDto, Data.Basket>().ReverseMap();
        CreateMap<BasketItemDto, BasketItem>().ReverseMap();
    }
}