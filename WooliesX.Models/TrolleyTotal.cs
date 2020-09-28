using System.Collections.Generic;
using System.Linq;

namespace WooliesX.Models
{
    public class TrolleyTotal
    {
        public List<TrolleyTotal> Children { get; set; } = new List<TrolleyTotal>();
        public Dictionary<string, Product> RemainProduct { get; set; }
        public List<Special> UsedSpecials { get; set; }

        public List<Special> AvailableSpecials { get; set; }

        public decimal Total { get; set; }
        public decimal LowestTotal { get; set; }
        public decimal CalTotal()
        {
            decimal total = 0;
            foreach (var item in RemainProduct)
            {
                total += item.Value.Price * (decimal)item.Value.Quantity;
            }
            foreach (var item in UsedSpecials)
            {
                total += item.Total;
            }
            return total;
        }
        public List<Special> GetValidSpecial(List<Special> specials)
        {
            return specials.Where(x => x.Quantities.All(quantity => RemainProduct.ContainsKey(quantity.Name)
                                                        && RemainProduct[quantity.Name].Quantity >= quantity.Quantity)).ToList();
        }

        public decimal getLowestTotal()
        {
            if (!AvailableSpecials.Any())
            {
                LowestTotal = Total;
                return LowestTotal;
            }
            foreach (var item in AvailableSpecials)
            {
                var node = new TrolleyTotal()
                {
                    RemainProduct = RemainProduct.ToDictionary(t => t.Key, t => new Product { Name = t.Value.Name, Price = t.Value.Price, Quantity = t.Value.Quantity }),
                    UsedSpecials = new List<Special>(UsedSpecials)
                };
                node.UsedSpecials.Add(item);
                foreach (var productItem in item.Quantities)
                {
                    node.RemainProduct[productItem.Name].Quantity -= productItem.Quantity;
                }
                node.Total = node.CalTotal();
                node.AvailableSpecials = node.GetValidSpecial(AvailableSpecials);
                Children.Add(node);
            }
            foreach (var child in Children)
            {
                child.getLowestTotal();
                if (Total > child.LowestTotal)
                {
                    LowestTotal = child.LowestTotal;
                }
            }
            return LowestTotal;
        }
    }
}
