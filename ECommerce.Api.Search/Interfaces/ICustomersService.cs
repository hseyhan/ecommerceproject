using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Search.Model;

namespace ECommerce.Api.Search.Interfaces
{
    public interface ICustomersService
    {
        Task<(bool IsSucess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int id);
    }
}
