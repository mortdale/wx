using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WooliesX.Functions;
using WooliesX.Models;
using WooliesX.Services;

[assembly: FunctionsStartup(typeof(Startup))]
namespace WooliesX.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
            builder.Services.AddScoped<IProductServices, ProductServices>();
            builder.Services.AddScoped<ITrolleyService, TrolleyService>();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IResourceServices, ResourceServices>();
            //builder.Services.Configure<Settings>(config.GetSection("Values"));

            builder.Services.AddOptions<Settings>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.Bind(settings); });
        }
    }
}
