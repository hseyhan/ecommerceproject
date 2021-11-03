using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Orders.Models;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrderProvider
    {
        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync();
        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId);
    }
}
