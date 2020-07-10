namespace Corona.Crawler
{
    using HtmlAgilityPack;
    using HtmlAgilityPack.CssSelectors.NetCore;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public sealed class Worldmeter
    {
        public const string Url = "https://www.worldometers.info/coronavirus/country";

        public Worldmeter(string country) => this.Country = country;

        private string Country { get; }
        private string CountryUrl => $"{Url}/{this.Country}/";

        public async Task ProcessAsync()
        {
            using var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(this.CountryUrl);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);

            var processors = CountryProcessors.GetProcessors(this.Country);

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