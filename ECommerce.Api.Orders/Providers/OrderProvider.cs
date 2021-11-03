using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order = ECommerce.Api.Orders.Models.Order;

namespace ECommerce.Api.Orders.Providers
{
    public class OrderProvider : IOrderProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrderProvider> logger;
        private readonly IMapper mapper;

        public OrderProvider(OrdersDbContext dbContext, ILogger<OrderProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        public async Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync()
        {
            try
            {
                var orders = await dbContext.Orders.ToListAsync();

                if (!orders.Any()) return (false, null, "No found");

                var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Order>>(orders);

                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Include(i=> i.Items).Where(s=> s.CustomerId == customerId).ToListAsync();

                if (!orders.Any()) return (false, null, "No found");

                var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Order>>(orders); ;

                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private void SeedData()
        {
            if (!dbContext.OrderItems.Any())
            {
                dbContext.OrderItems.Add(new Db.OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 4, UnitPrice = 23 });
                dbContext.OrderItems.Add(new Db.OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 5, UnitPrice = 43 });
                dbContext.OrderItems.Add(new Db.OrderItem { Id = 3, OrderId = 1, ProductId = 3, Quantity = 6, UnitPrice = 55 });
                dbContext.SaveChanges();
            }

            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Today,
                    Items = dbContext.OrderItems.ToListAsync().Result
                });


                dbContext.SaveChanges();
            }
        }
    }
}
