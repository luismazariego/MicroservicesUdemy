namespace Catalog.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Repositories.Interfaces;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository,
                                ILogger<CatalogController> logger)
        {
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger)); ;
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            IEnumerable<Product> products = await _repository.GetProductsAsync();

            return Ok(products);
        }
        
        [HttpGet("{id:length(24)}", Name="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.GetProductAsync(id);

            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{categoryName}")]
        [HttpGet]        
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string categoryName)
        {
            var products = await _repository.GetProductsByCategoryAsync(categoryName);

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]   
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await _repository.Create(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Product product)
        {
            return Ok(await _repository.Update(product));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteById(string id)
        {
            return Ok(await _repository.Delete(id));
        }

    }
}