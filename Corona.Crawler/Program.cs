namespace Corona.Crawler
{
    using System;
    using System.Threading.Tasks;

    internal class Program
    {
        private static async Task Main()
        {
            await new Worldmeter("Romania").ProcessAsync();

            Console.ReadKey();
        }
    }
}