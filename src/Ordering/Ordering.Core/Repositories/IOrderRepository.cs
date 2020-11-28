namespace Ordering.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Base;

    using Entities;

    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName);
    }
}
