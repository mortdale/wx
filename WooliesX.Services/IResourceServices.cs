using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesX.Models;

namespace WooliesX.Services
{
    public interface IResourceServices
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<IEnumerable<ShopperHistory>> GetShopperHistory();

        Task<decimal> GetTrolleyTotal(Trolley trolley);
    }
}
