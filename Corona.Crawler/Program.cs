namespace Corona.Crawler
{
    using System;
    using System.Threading.Tasks;
    using Corona.Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Corona.Domain.Extensions;
    using Microsoft.Extensions.Configuration;

    internal class Program
    {
        private static async Task Main()
        {
            var serviceProvider = BuildServiceProvider();

            var worldmeter = serviceProvider.GetRequiredService<IWorldmeter>();

            await worldmeter
                .ExecuteFor("Romania")
                .ProcessAsync();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<StorageContext>(options =>
                    options.UseMySQL(configuration["ConnectionStrings:corona"])
                 )
                .AddScoped<StorageContext>()
                .AddSingleton<IWorldmeter, Worldmeter>()
                .AddSingleton<ICountryProcessors, CountryProcessors>()
               ;

            AppDomain.CurrentDomain.GetTypesAssignableFrom<IProcessor>()
                .ForEach(x =>
                {
                    serviceProvider.AddScoped(typeof(IProcessor), x);
                });

            return serviceProvider.BuildServiceProvider();
        }
    }
}