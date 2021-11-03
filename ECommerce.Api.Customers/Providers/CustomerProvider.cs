using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Customer = ECommerce.Api.Customers.Models.Customer;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomerProvider : ICustomerProvider
    {
        private readonly CustomerDbContext dbContext;
        private readonly ILogger<CustomerProvider> logger;
        private readonly IMapper mapper;

        public CustomerProvider(CustomerDbContext dbContext, ILogger<CustomerProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedDate();
        }

        public async Task<(bool IsSuccess, IEnumerable<Customer> Products, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();

                if (!customers.Any()) return (false, null, "No found");

                var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Customer>>(customers);

                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Customer Product, string ErrorMessage)> GetCustomersAsync(int id)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c=> c.Id == id);

                if (customer == null) return (false, null, "No found");

                var result = mapper.Map<Db.Customer, Models.Customer>(customer);

                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private void SeedDate()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer { Id = 1, Name = "Hürkan", Address = "Address 1" });
                dbContext.Customers.Add(new Db.Customer { Id = 2, Name = "Duygu", Address = "Address 2" });
                dbContext.Customers.Add(new Db.Customer { Id = 3, Name = "Ahmet", Address = "Address 3" });
                dbContext.Customers.Add(new Db.Customer { Id = 4, Name = "Buse", Address = "Address 4" });
                dbContext.SaveChanges();
            }
        }
    }
}
