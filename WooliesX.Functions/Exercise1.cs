using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WooliesX.Functions
{
    public class Exercise1
    {
        [FunctionName("Exercise1")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "answers/user")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Exercise1 triggered.");

            var response = new
            {
                Name = @"Yangkang Tang",
                Token = @"d52c71ea-7b3a-4b3f-9933-cc537441d0ed"
            };

            return new JsonResult(response);
        }
    }
}
