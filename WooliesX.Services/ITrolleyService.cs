using System.Threading.Tasks;
using WooliesX.Models;

namespace WooliesX.Services
{
    public interface ITrolleyService
    {
        Task<decimal> GetTotal(Trolley trolley);

        decimal CalculateTotal(Trolley trolley);
    }
}
