using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext _customerDbContext;
        private readonly ILogger<CustomersProvider> _logger;
        private readonly IMapper _mapper;

        public CustomersProvider(CustomersDbContext customerDbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            _customerDbContext = customerDbContext;
            _logger = logger;
            _mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!_customerDbContext.Customers.Any())
            {
                _customerDbContext.Customers.Add(new Customer{Id = 1, Name = "Bob Smith",  Address = "1 Leaf Street, Rondebosh"});
                _customerDbContext.Customers.Add(new Customer{Id = 2, Name = "Sandy Thorn",  Address = "20 Bushbuck Street, Kenilworth"});
                _customerDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<CustomerModel> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await _customerDbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>(customers);
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

        public async Task<(bool IsSuccess, CustomerModel Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await _customerDbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
                if (customer != null)
                {
                    var result = _mapper.Map<Customer, CustomerModel>(customer);
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
