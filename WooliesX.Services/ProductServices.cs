using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesX.Models;

namespace WooliesX.Services
{
    public class ProductServices: IProductServices
    {
        private readonly IResourceServices _apiServices;

        public ProductServices(IResourceServices apiServices)
        {
            _apiServices = apiServices;
        }

        public async Task<IEnumerable<Product>> GetProductsSortBy(string sortOption)
        {
            var products = await _apiServices.GetProducts();

            if (Enum.TryParse(sortOption, true, out SortOptions option))
            {   
                if (option == SortOptions.Recommended)
                {   
                    var history = await _apiServices.GetShopperHistory();
                    return SortByPopularity(products, history);
                }

                switch (option)
                {
                    case SortOptions.Low:
                        return products.OrderBy(t => t.Price);
                    case SortOptions.High:
                        return products.OrderByDescending(t => t.Price);
                    case SortOptions.Ascending:
                        return products.OrderBy(t => t.Name);
                    case SortOptions.Descending:
                        return products.OrderByDescending(t => t.Name);
                }
            }

            return products;
        }

        private IEnumerable<Product> SortByPopularity(IEnumerable<Product> products,
            IEnumerable<ShopperHistory> history)
        {
            var records = history.SelectMany(h => h.Products)
                .GroupBy(t => t.Name)
                .Select(group => new {group.Key, count = group.Sum(item => item.Quantity)})
                .OrderByDescending(t => t.count).ToList();

            var dict = products.ToDictionary(t => t.Name, t => t);

            var result = new List<Product>();

            foreach (var record in records)
            {
                if (dict.ContainsKey(record.Key))
                {
                    result.Add(dict[record.Key]);
                    dict.Remove(record.Key);
                }

                if (dict.Count == 0) break;
            }

            result.AddRange(dict.Values);

            return result;
        }
    }
}
