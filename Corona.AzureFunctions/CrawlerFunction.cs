namespace Corona.AzureFunctions
{
    using System;
    using System.Threading.Tasks;
    using Corona.Crawler;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public static class CrawlerFunction
    {
        [FunctionName(nameof(CrawlerFunction))]
        public static async Task RunAsync([TimerTrigger("0 0 23 * * *")] TimerInfo myTimer, ILogger log)
        {
            await new Worldmeter("Romania").ProcessAsync();

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
