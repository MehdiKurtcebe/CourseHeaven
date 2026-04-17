using AutoMapper;
using CourseHeaven.Order.Application.Features.Orders.CreateOrder;
using CourseHeaven.Order.Domain.Entities;

namespace CourseHeaven.Order.Application.Features.Orders;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
    }
}