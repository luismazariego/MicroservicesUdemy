namespace Ordering.Application.Mapper
{
    using AutoMapper;

    using Core.Entities;

    using Commands;

    using Responses;

    internal class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
        }
    }
}