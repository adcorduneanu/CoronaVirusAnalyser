namespace Corona.Crawler
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    class CountryProcessors
    {
        public static IEnumerable<IProcessor> GetProcessors(string country) => Processors.Value[country];

        private static readonly Lazy<ILookup<string, IProcessor>> Processors
            = new Lazy<ILookup<string, IProcessor>>(
                () =>
                {
                    var processorType = typeof(IProcessor);
                    var processors = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .Where(x => processorType.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                        .Select(x => (IProcessor)Activator.CreateInstance(x));

                    return processors.ToLookup(x => x.Country);
                }
            );
    }
}
