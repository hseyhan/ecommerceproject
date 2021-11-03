using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderProvider orderProvider;

        public OrderController(IOrderProvider orderProvider)
        {
            this.orderProvider = orderProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var result = await orderProvider.GetOrdersAsync();
            if (!result.IsSuccess) return NotFound();
            return Ok(result.Orders);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrdersAsync(int customerId)
        {
            var result = await orderProvider.GetOrdersAsync(customerId);
            if (!result.IsSuccess) return NotFound();
            return Ok(result.Orders);
        }

    }
}
