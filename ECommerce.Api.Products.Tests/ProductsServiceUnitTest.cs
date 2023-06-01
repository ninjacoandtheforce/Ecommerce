using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceUnitTest
    {
        [Fact]
        public async Task GetProductsReturnAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(ProductsServiceUnitTest)).Options;
            var dbContext = new ProductsDbContext(options);
            
            CreateProducts(dbContext);
            
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            
            var result = await productsProvider.GetProductsAsync();
            Assert.True(result.IsSuccess);
            Assert.True(result.Products.Any());
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task GetProductsReturnProductValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnProductValidId)).Options;
            var dbContext = new ProductsDbContext(options);

            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var result = await productsProvider.GetProductAsync(1);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Product);
            Assert.True(result.Product.Id == 1);
            Assert.Null(result.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product {  Id = i, Name = Guid.NewGuid().ToString(), Inventory = i + 10, Price = (decimal)(i * 3.41)});
                dbContext.SaveChanges();
            }
        }
    }
}