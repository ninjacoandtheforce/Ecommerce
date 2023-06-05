using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider _productsProvider;

        public ProductsController(IProductsProvider productsProvider)
        {
            _productsProvider = productsProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await _productsProvider.GetProductsAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Products);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await _productsProvider.GetProductAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }
            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var result = await _productsProvider.DeleteProductAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostProductAsync(ProductModel product)
        {
            var result = await _productsProvider.PostProductAsync(product);
            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
