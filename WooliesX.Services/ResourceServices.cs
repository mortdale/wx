using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WooliesX.Models;

namespace WooliesX.Services
{
    public class ResourceServices : BaseRequest, IResourceServices
    {
        public ResourceServices(IOptions<Settings> option, IHttpClientFactory clientFactory) : base(option,
            clientFactory)
        {
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var url = new Uri($"{Settings.BaseUrl}/products");

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await SendAsync(request).ConfigureAwait(false);
            
            var json = await response.Content.ReadAsStringAsync();
            
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

            return products;
        }

        public async Task<IEnumerable<ShopperHistory>> GetShopperHistory()
        {
            var url = new Uri($"{Settings.BaseUrl}/shopperHistory");

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await SendAsync(request).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync();
            
            var history = JsonConvert.DeserializeObject<IEnumerable<ShopperHistory>>(json);

            return history;
        }

        public async Task<decimal> GetTrolleyTotal(Trolley trolley)
        {
            var url = new Uri($"{Settings.BaseUrl}/trolleyCalculator");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = GenerateJsonContent(trolley)
            };

            var response = await SendAsync(request).ConfigureAwait(false);

            var total = await response.Content.ReadAsStringAsync();

            return Convert.ToDecimal(total);
        }
    }
}
