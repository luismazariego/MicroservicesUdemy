namespace Basket.Api.Mapping
{
    using AutoMapper;

    using Basket.Api.Entities;

    using EventBusRabbitMQ.Events;

    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
