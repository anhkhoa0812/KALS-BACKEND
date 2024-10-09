using AutoMapper;
using KALS.API.Models.OrderItem;
using KALS.Domain.Entities;

namespace KALS.API.Mapper;

public class OrderItemMapper: Profile
{
    public OrderItemMapper()
    {
        CreateMap<OrderItem, OrderItemResponse>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
    }
}