using System;
using Corona.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Corona.AzureFunctions.Startup))]

namespace Corona.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<StorageContext>(options =>
               options.UseMySQL(Environment.GetEnvironmentVariable("ConnectionStrings:corona"))
           );
        }
    }
}