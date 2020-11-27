namespace Basket.Api.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Entities;

    using EventBusRabbitMQ.Common;
    using EventBusRabbitMQ.Events;
    using EventBusRabbitMQ.Producer;

    using Microsoft.AspNetCore.Mvc;

    using Repositories.Interfaces;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBus;

        public BasketController(IBasketRepository repository,
            IMapper mapper, EventBusRabbitMQProducer producer)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
            _eventBus = producer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
            return Ok(await _repository.GetBasket(userName) ?? new BasketCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket(BasketCart basket)
        {
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket(string userName)
        {
            return Ok(await _repository.DeleteBasket(userName));
        }

        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout(BasketCheckout basketCheckout)
        {
            //get total price of basket
            //remove basket
            //send checkout event to rabbitmq
            BasketCart basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null) return BadRequest();

            bool isBasketRemoved = await _repository.DeleteBasket(basket.UserName);
            if (!isBasketRemoved) return BadRequest();

            BasketCheckoutEvent eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                _eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueueName, eventMessage);
            }
            catch (Exception)
            {
                throw;
            }

            return Accepted();
        }
    }
}
