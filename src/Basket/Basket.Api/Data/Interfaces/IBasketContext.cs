namespace Basket.Api.Data.Interfaces
{
    using StackExchange.Redis;
    
    public interface IBasketContext 
    {
        IDatabase Redis { get; }     
    }
}