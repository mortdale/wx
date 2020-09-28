using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesX.Models;

namespace WooliesX.Services
{
    public interface IProductServices
    {
        Task<IEnumerable<Product>> GetProductsSortBy(string sortOption);
    }
}
