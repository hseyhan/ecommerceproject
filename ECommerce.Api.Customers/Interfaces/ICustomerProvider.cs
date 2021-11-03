﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Customers.Models;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomerProvider
    {
        Task<(bool IsSuccess, IEnumerable<Customer> Products, string ErrorMessage)> GetCustomersAsync();
        Task<(bool IsSuccess, Customer Product, string ErrorMessage)> GetCustomersAsync(int id);
    }
}
