using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Db
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public CustomerDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
