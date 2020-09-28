using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WooliesX.Services;

namespace WooliesX.Functions
{
    public class Exercise2
    {
        private readonly IProductServices _productServices;

        public Exercise2(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [FunctionName("Exercise2")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "answers/sort")] HttpRequest req,
            ILogger log)
        {   
            log.LogInformation("Exercise2 triggered.");

            string sortOption = req.Query["sortOption"];

            var products = await _productServices.GetProductsSortBy(sortOption);

            return new OkObjectResult(products);
        }
    }
}
