namespace Catalog.Api.Data.Interfaces
{
    using Entities;
    using MongoDB.Driver;

    public interface ICatalogContext
    {
         IMongoCollection<Product> Products { get; }
    }
}