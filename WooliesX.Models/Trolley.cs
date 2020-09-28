using System.Collections.Generic;

namespace WooliesX.Models
{
    public class Trolley
    {
        public IEnumerable<BaseProduct> Products { get; set; }

        public IEnumerable<Special> Specials { get; set; }

        public IEnumerable<ProductQuantity> Quantities { get; set; }
    }
}
