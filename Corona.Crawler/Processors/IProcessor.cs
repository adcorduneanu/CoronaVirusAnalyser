namespace Corona.Crawler
{
    using System.Threading.Tasks;

    internal interface IProcessor
    {
        public string Country { get; }
        Task ProcessAsync(string url);
    }
}