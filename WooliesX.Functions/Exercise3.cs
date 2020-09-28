using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WooliesX.Services;
using System.IO;
using Newtonsoft.Json;
using WooliesX.Models;

namespace WooliesX.Functions
{
    public class Exercise3
    {
        private readonly ITrolleyService _trolleyServices;

        public Exercise3(ITrolleyService trolleyServices)
        {
            _trolleyServices = trolleyServices;
        }

        [FunctionName("Exercise3")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "answers/trolleyTotal")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Exercise3 triggered.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var trolley = JsonConvert.DeserializeObject<Trolley>(requestBody);

            string isExpert = req.Query["isExpert"];

            log.LogInformation(JsonConvert.SerializeObject(trolley, Formatting.Indented));

            decimal total;

            //if ((isExpert ?? "").Equals("true", StringComparison.OrdinalIgnoreCase))
                total = _trolleyServices.CalculateTotal(trolley);
            //else
              //  total = await _trolleyServices.GetTotal(trolley);


            return new OkObjectResult(total);
        }
    }
}
