using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesX.Models;

namespace WooliesX.Services
{
    public class TrolleyService : ITrolleyService
    {
        private readonly IResourceServices _apiServices;

        public TrolleyService(IResourceServices apiServices)
        {
            _apiServices = apiServices;
        }

        public async Task<decimal> GetTotal(Trolley trolley)
        {
            var total = await _apiServices.GetTrolleyTotal(trolley);

            return total;
        }

        public decimal CalculateTotal(Trolley trolley)
        {
            var unitPrices = trolley.Products.ToDictionary(p => p.Name, p => p.Price);

            var specials = new Dictionary<string, List<ProductSpecial>>();

            foreach (var trolleySpecial in trolley.Specials)
            foreach (var trolleySpecialQuantity in trolleySpecial.Quantities)
            {
                if (trolleySpecialQuantity.Quantity == 0) continue;

                if (!specials.ContainsKey(trolleySpecialQuantity.Name))
                {
                    specials[trolleySpecialQuantity.Name] = new List<ProductSpecial>();
                }
                var productSpecial = new ProductSpecial
                    { Quantity = trolleySpecialQuantity.Quantity, SpecialUnitPrice = trolleySpecial.Total };

                specials[trolleySpecialQuantity.Name].Add(productSpecial);
            }

            decimal total = 0;

            foreach (var trolleyQuantity in trolley.Quantities)
            {
                var lowestTotal = CalculateTotalPrice(trolleyQuantity.Quantity, specials[trolleyQuantity.Name], unitPrices[trolleyQuantity.Name]);
                total += lowestTotal;
            }

            return total;
        }

        private static decimal CalculateTotalPrice(int qty, IEnumerable<ProductSpecial> specials, decimal unitPrice)
        {
            if (qty == 0) return 0;

            if (specials == null) return qty * unitPrice;

            var remaining = qty;
            decimal total = 0;

            foreach (var productSpecial in specials.OrderByDescending(s=>s.SpecialUnitPrice))
            {
                var group = remaining / productSpecial.Quantity;
                remaining = remaining % productSpecial.Quantity;

                total += group * productSpecial.SpecialUnitPrice;

                if (remaining == 0) break;
            }

            if (remaining > 0)
                total += remaining * unitPrice;

            return total;
        }
    }
}
