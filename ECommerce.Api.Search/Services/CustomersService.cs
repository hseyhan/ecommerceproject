using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Model;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomersService
    {
        private IHttpClientFactory httpClientFactory;
        private ILogger<OrdersService> logger;

        public CustomersService(IHttpClientFactory httpClientFactory, ILogger<OrdersService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<(bool IsSucess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var client = httpClientFactory.CreateClient("CustomersService");
                var response = await client.GetAsync($"api/customers/{id}");


                if (!response.IsSuccessStatusCode) return (false, null, response.ReasonPhrase);

                var content = await response.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<dynamic>(content, options);
                return (true, result, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
