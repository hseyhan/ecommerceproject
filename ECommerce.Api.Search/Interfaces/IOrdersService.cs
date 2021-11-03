using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Search.Model;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IOrdersService
    {
        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId);
    }
}
