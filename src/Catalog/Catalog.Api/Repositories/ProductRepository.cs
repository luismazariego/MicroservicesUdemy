namespace Catalog.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Interfaces;
    using Entities;
    using Interfaces;
    using MongoDB.Driver;

    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context 
                ?? throw new ArgumentNullException(nameof(context)); 
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>
                                                .Filter
                                                .ElemMatch(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                              .Products
                                              .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                    && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProductAsync(string id)
            => await _context
                    .Products
                    .Find(p => p.Id == id)
                    .FirstOrDefaultAsync();


        public async Task<IEnumerable<Product>> GetProductsAsync() 
            => await _context
                    .Products
                    .Find(p => true)
                    .ToListAsync();

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>
                                                .Filter
                                                .ElemMatch(p => p.Category, categoryName);

            return await _context
                        .Products
                        .Find(filter)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>
                                                .Filter
                                                .ElemMatch(p => p.Name, name);

            return await _context
                        .Products
                        .Find(filter)
                        .ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context
                                    .Products
                                    .ReplaceOneAsync(
                                        filter: 
                                        g => g.Id == product.Id, 
                                        replacement: product
                                    );
            return updateResult.IsAcknowledged 
                    && updateResult.MatchedCount > 0;
        }
    }
}