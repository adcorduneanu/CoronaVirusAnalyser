namespace Corona.Crawler
{
    using System.Threading.Tasks;

    public interface IWorldmeter
    {
        IWorldmeter ExecuteFor(string country);
        Task ProcessAsync();
    }
}