namespace Corona.App.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Corona.Domain.Providers;
    using Corona.Domain.ValueObjects;
    using Corona.Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Country = Domain.Entities.Country;
    using Locality = Domain.Entities.Locality;

    public sealed class CoronaDataProvider : ICoronaDataProvider
    {
        private readonly StorageContext storageContext;

        public CoronaDataProvider(StorageContext storageContext)
        {
            this.storageContext = storageContext;
        }
        public async Task<IList<Country>> GetCountries()
        {
            var countries = await this.storageContext.Countries
                .Include(x => x.Localities)
                .Select(x => new Country
                {
                    Name = x.Name,
                    Localities = x.Localities
                        .OrderBy(x => x.Name)
                        .Select(y => y.Name)
                        .ToList()
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return countries;
        }

        public async Task<IEnumerable<Locality>> GetLocalitiesData()
        {
            var locations = await this.storageContext.Localities
                .Include(x => x.Country)
                .Include(x => x.Sicks)
                .ToListAsync();

            return locations.Select(GetLocality)
                .OrderByDescending(x=>x.DateValues.Sum(y=>y.Difference))
                .ToList();
        }

        public async Task<Locality> GetLocalityData(string country, string locality)
        {
            var location = await this.storageContext.Localities
                .Include(x => x.Country)
                .Include(x => x.Sicks)
                .FirstOrDefaultAsync(x => x.Name.ToLower() == locality.ToLower()
                    && x.Country.Name.ToLower() == country.ToLower()
                );

            return GetLocality(location);
        }

        private static Locality GetLocality(Infrastructure.Locality location)
        {
            var dateValues = new List<DateValuePair>();
            location.Sicks
                .OrderBy(x => x.Date)
                .ToList()
                .ForEach(x =>
                {
                    var previousValue = dateValues.Any()
                        ? dateValues.Last().Value
                        : x.Records;
                    dateValues.Add(new DateValuePair(x.Date, x.Records, previousValue));
                });

            return new Locality(location.Name, location.Country.Name)
                .WithDates(dateValues);
        }
    }
}
