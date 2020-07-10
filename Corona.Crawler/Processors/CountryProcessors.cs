namespace Corona.Crawler
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public class CountryProcessors : ICountryProcessors
    {
        public CountryProcessors(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IProcessor> GetProcessors(string country) => this.Processors.Value[country];

        private readonly Lazy<ILookup<string, IProcessor>> Processors =
            new Lazy<ILookup<string, IProcessor>>(() =>
                _serviceProvider.GetServices<IProcessor>()
                    .ToLookup(x => x.Country)
            );

        private static IServiceProvider _serviceProvider;
    }
}
