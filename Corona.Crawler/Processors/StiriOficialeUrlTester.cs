namespace Corona.Crawler
{
    using System;
    using System.Threading.Tasks;
    using Corona.Infrastructure;

    public class StiriOficialeUrlTester : IProcessor
    {
        private readonly StorageContext storageContext;

        public string Country => "Romania";

        public StiriOficialeUrlTester(StorageContext storageContext)
        {
            this.storageContext = storageContext;
        }

        public async Task ProcessAsync(string url)
        {
            Console.WriteLine(url);

            await Task.Delay(0);
        }
    }
}