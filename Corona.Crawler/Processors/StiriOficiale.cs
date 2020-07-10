namespace Corona.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using Corona.Infrastructure;
    using HtmlAgilityPack;
    using HtmlAgilityPack.CssSelectors.NetCore;

    public class StiriOficiale : IProcessor
    {
        private readonly StorageContext storageContext;

        public string Country => "Romania";

        public StiriOficiale(StorageContext storageContext)
        {
            this.storageContext = storageContext;
        }

        public async Task ProcessAsync(string url)
        {
            var sickDateTime = this.ParseForDate(url);

            this.storageContext.Database.EnsureCreated();

            var existingData = this.storageContext.Sicks
                .Any(x => x.Date == sickDateTime && x.Locality.Country.Name == Country);

            if (existingData)
            {
                return;
            }

            Country country = await ProcessCountryData();

            var records = await GetSourceRecords(url);

            records.ToList()
                .ForEach(localRecord =>
                {
                    ProcessLocalData(url, localRecord, sickDateTime, country);
                });


            await this.storageContext.SaveChangesAsync();
        }

        private async Task<Country> ProcessCountryData()
        {
            var country = this.storageContext.Countries
                .FirstOrDefault(x => x.Name == this.Country);

            if (country is null)
            {
                country = new Country
                {
                    Name = this.Country,
                    Localities = new List<Locality>()
                };

                await this.storageContext.Countries
                    .AddAsync(country);
            }

            return country;
        }

        private void ProcessLocalData(string url, (string locality, int records) localRecord, DateTime sickDateTime, Country country)
        {
            var locality = this.storageContext.Localities
                .FirstOrDefault(y => y.Name == localRecord.locality && y.Country.Id == country.Id);

            if (locality is null)
            {
                locality = new Locality
                {
                    Country = country,
                    Name = localRecord.locality,
                    Sicks = new List<Sick>()
                };

                this.storageContext.Localities
                    .Add(locality);
            }

            var sick = new Sick
            {
                Date = sickDateTime,
                Locality = locality,
                SourceUrl = url,
                Records = localRecord.records
            };

            this.storageContext.Sicks
                .Add(sick);
        }

        private async Task<IEnumerable<(string locality, int records)>> GetSourceRecords(string url)
        {
            var content = await GetContent(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);

            var records = htmlDocument.QuerySelector("table")
                .QuerySelectorAll("tr")
                .Skip(1)
                .Reverse()
                .Skip(1)
                .Reverse()
                .ToList()
                .Select(x =>
                {
                    var children = x.QuerySelectorAll("td")
                        .Skip(1);
                    try
                    {
                        var locality = GetLocality(children);
                        var value = int.Parse(GetRecords(children)
                        );

                        return (locality, value);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                });

            return records;
        }

        private static string GetRecords(IEnumerable<HtmlNode> children) => Regex.Replace(
                HttpUtility.HtmlDecode(
                    children.Last()
                    .InnerText
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace(".", string.Empty)
                ),
                @"\s+",
                string.Empty
            );

        private static string GetLocality(IEnumerable<HtmlNode> children) => HttpUtility.HtmlDecode(
                children.First()
                .InnerText
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
            );

        private DateTime ParseForDate(string url)
        {
            var dateString = url.Replace("https://stirioficiale.ro/informatii/buletin-de-presa-", string.Empty)
                .Replace("-ora-", " ");

            return DateTime.ParseExact(
                dateString,
                @"d-MMMM-yyyy HH-mm",
                new CultureInfo("ro-RO")
            );
        }

        private async Task<string> GetContent(string url)
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetStringAsync(url);
        }
    }
}