namespace Corona.Crawler
{
    using System.Collections.Generic;

    public interface ICountryProcessors
    {
        IEnumerable<IProcessor> GetProcessors(string country);
    }
}