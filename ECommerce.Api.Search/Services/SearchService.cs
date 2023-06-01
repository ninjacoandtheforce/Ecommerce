using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService _orderService;
        private readonly IProductsService _productsService;
        private readonly ICustomersService _customersService;

        public SearchService(IOrderService orderService, IProductsService productsService, ICustomersService customersService)
        {
            _orderService = orderService;
            _productsService = productsService;
            _customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await _orderService.GetOrdersAsync(customerId);
            var productsResult = await _productsService.GetProductsAsync();
            var customersResult = await _customersService.GetCustomerAsync(customerId);

            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    order.CustomerName = customersResult.IsSuccess
                        ? customersResult.Customer?.Name
                        : "Customer information is not available;";
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess
                            ? productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name
                            : "Product information is not available.";
                    }    
                }
                var result = new
                {
                    Customer = customersResult.IsSuccess ?
                        customersResult.Customer :
                        new Customer{ Name = "Customer information is not available" },
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
