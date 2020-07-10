namespace Corona.Crawler
{
    using System.Threading.Tasks;

    public interface IProcessor
    {
        public string Country { get; }
        Task ProcessAsync(string url);
    }
}