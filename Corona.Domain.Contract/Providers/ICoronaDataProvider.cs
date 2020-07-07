namespace Corona.Domain.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Corona.Domain.Entities;

    public interface ICoronaDataProvider
    {
        Task<IList<Country>> GetCountries();
        Task<Locality> GetLocalityData(string country, string locality);
        Task<IEnumerable<Locality>> GetLocalitiesData();
    }
}
