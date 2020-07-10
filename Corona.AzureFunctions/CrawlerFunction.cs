namespace Corona.AzureFunctions
{
    using System;
    using System.Threading.Tasks;
    using Corona.Crawler;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public class CrawlerFunction
    {
        private readonly IWorldmeter worldmeter;

        public CrawlerFunction(IWorldmeter worldmeter)
        {
            this.worldmeter = worldmeter;
        }

        [FunctionName(nameof(CrawlerFunction))]
        public async Task RunAsync([TimerTrigger("0 0 23 * * *")] TimerInfo myTimer, ILogger log)
        {
            await this.worldmeter.ExecuteFor("Romania").ProcessAsync();

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
