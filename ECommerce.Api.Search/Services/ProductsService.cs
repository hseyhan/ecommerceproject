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
    public class ProductsService : IProductsService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ProductsService> logger;

        public ProductsService(IHttpClientFactory httpClientFactory, ILogger<ProductsService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient("ProductsService");
                var response = await client.GetAsync("api/products");

                if (!response.IsSuccessStatusCode)
                    return (false, null, response.ReasonPhrase);

                var content = await response.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<IEnumerable<Product>>(content, options);
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
