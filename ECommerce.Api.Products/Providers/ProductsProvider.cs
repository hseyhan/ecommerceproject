using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product = ECommerce.Api.Products.Models.Product;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;
        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedDate();
        }

        private void SeedDate()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product { Id = 1, Name = "Keyboard", Price = 20, Invertory = 100 });
                dbContext.Products.Add(new Db.Product { Id = 2, Name = "Mouse", Price = 5, Invertory = 200 });
                dbContext.Products.Add(new Db.Product { Id = 3, Name = "Monitor", Price = 150, Invertory = 100 });
                dbContext.Products.Add(new Db.Product { Id = 4, Name = "CPU", Price = 200, Invertory = 2000 });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if (!products.Any()) return (false, null, "Not found");

                var result = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
                return (true, result, null);

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Product Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p=> p.Id == id);
                if (product == null) return (false, null, "Not found");

                var result = mapper.Map<Db.Product, Models.Product>(product);
                return (true, result, null);

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
