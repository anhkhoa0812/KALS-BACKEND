using AutoMapper;
using KALS.API.Models.Payment;
using KALS.Domain.Entities;

namespace KALS.API.Mapper;

public class PaymentMapper: Profile
{
    public PaymentMapper()
    {
        CreateMap<Payment, PaymentWithOrderResponse>()
            .ForMember(dest => dest.Order, 
                opt => opt.MapFrom(src => src.Order));
    }
}