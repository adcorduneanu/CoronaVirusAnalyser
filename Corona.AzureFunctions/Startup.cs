using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Corona.AzureFunctions.Startup))]

namespace Corona.AzureFunctions
{
    using System;
    using Corona.Crawler;
    using Corona.Domain.Extensions;
    using Corona.Infrastructure;
    using Microsoft.EntityFrameworkCore;    
    using Microsoft.Extensions.DependencyInjection;

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddDbContext<StorageContext>(options =>
                   options.UseMySql(
                       Environment.GetEnvironmentVariable("ConnectionStrings:corona")
                    )
                )
                .AddSingleton<StorageContext>()
                .AddSingleton<IWorldmeter, Worldmeter>()
                .AddSingleton<ICountryProcessors, CountryProcessors>();

            AppDomain.CurrentDomain.GetTypesAssignableFrom<IProcessor>()
                .ForEach(x =>
                {
                    builder.Services.AddScoped(typeof(IProcessor), x);
                });
        }
    }
}