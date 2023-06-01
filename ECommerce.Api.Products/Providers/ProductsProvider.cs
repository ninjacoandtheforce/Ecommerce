using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext _productsDbContext;
        private readonly ILogger<ProductsProvider> _logger;
        private readonly IMapper _mapper;

        public ProductsProvider(ProductsDbContext productsDbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            _productsDbContext = productsDbContext;
            _logger = logger;
            _mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!_productsDbContext.Products.Any())
            {
                _productsDbContext.Products.Add(new Product { Id = 1, Name = "Keyboard", Inventory = 1, Price = 10 });
                _productsDbContext.Products.Add(new Product { Id = 2, Name = "Monitor", Inventory = 1, Price = 150 });
                _productsDbContext.Products.Add(new Product { Id = 3, Name = "Mouse", Inventory = 1, Price = 5 });
                _productsDbContext.Products.Add(new Product { Id = 4, Name = "CPU", Inventory = 1, Price = 200 });
                _productsDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductModel> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await _productsDbContext.Products.ToListAsync();
                if (products != null && products.Any())
                {
                    var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(products);
                    return (true, result, null);
                }
                return(false, null, "Not Found");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }

        public async Task<(bool IsSuccess, ProductModel Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await _productsDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    var result = _mapper.Map<Product, ProductModel>(product);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
