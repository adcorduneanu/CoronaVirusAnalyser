namespace Corona.Crawler
{
    using HtmlAgilityPack;
    using HtmlAgilityPack.CssSelectors.NetCore;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public sealed class Worldmeter : IWorldmeter
    {
        private readonly string Url = "https://www.worldometers.info/coronavirus/country";
        private readonly ICountryProcessors countryProcessors;
        private string country = string.Empty;

        public Worldmeter(ICountryProcessors countryProcessors)
        {
            this.countryProcessors = countryProcessors;
        }

        private string CountryUrl => $"{Url}/{this.country}/";

        public IWorldmeter ExecuteFor(string country)
        {
            this.country = country;

            return this;
        }

        public async Task ProcessAsync()
        {
            using var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(this.CountryUrl);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);

            var processors = this.countryProcessors.GetProcessors(this.country);

            var links = htmlDocument.QuerySelectorAll(".news_source_a")
                .Select(x => x.Attributes.FirstOrDefault(y => y.Name == "href").Value)
                .SelectMany(link => processors.Select(processor => (link, processor)));

            foreach (var (link, processor) in links)
            {
                await processor.ProcessAsync(link);
            }
        }
    }
}