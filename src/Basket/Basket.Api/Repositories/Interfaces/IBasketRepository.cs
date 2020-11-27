namespace Basket.Api.Repositories.Interfaces
{
    using System.Threading.Tasks;
    using Entities;

    public interface IBasketRepository
    {
         Task<BasketCart> GetBasket(string userName);
        Task<BasketCart> UpdateBasket(BasketCart basket);
        Task<bool> DeleteBasket(string userName);
    }
}