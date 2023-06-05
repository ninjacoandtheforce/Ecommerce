using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSuccess, IEnumerable<ProductModel> Products, string ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, ProductModel Product, string ErrorMessage)> GetProductAsync(int id);
        Task<(bool IsSuccess, ProductModel Product, string ErrorMessage)> DeleteProductAsync(int id);
        Task<(bool IsSuccess, ProductModel Product, string ErrorMessage)> PostProductAsync(ProductModel product);
    }
}
