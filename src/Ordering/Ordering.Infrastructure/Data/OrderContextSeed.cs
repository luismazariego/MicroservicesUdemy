namespace Ordering.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using Ordering.Core.Entities;

    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, 
                                           ILoggerFactory loggerFactory, 
                                           int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                orderContext.Database.Migrate();

                if (!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetPreconfiguredOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                if (retryForAvailability < 3)
                {
                    retryForAvailability++;

                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError(e.Message);
                    await SeedAsync(orderContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order{ UserName = "ljm", FirstName="Luis", LastName="Mazariego", Country="El Salvador", AddressLine="Altos del Tejar", EmailAddress="mazariego2011@gmail.com"},
                new Order{ UserName = "rom", FirstName="Raul", LastName="Mazariego", Country="El Salvador", AddressLine="Altos del Tejar", EmailAddress="mazariego2020@gmail.com"},
                new Order{ UserName = "rfm", FirstName="Ranger", LastName="Mazariego", Country="El Salvador", AddressLine="Altos del Tejar", EmailAddress="mazariego2022@gmail.com"}
            };
        }
    }
}
