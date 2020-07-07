namespace Corona.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Corona.Domain.Entities;

    public class CountryViewModel
    {
       public string Country { get; private set; }
        public string Locality { get; private set; }
        public IEnumerable<Country> Countries { get; private set; }

        internal static Func<IList<Country>, CountryViewModel> ProjectFromDomain = countries =>
            new CountryViewModel
            {
                Countries = countries,
                Country = countries.FirstOrDefault()?.Name,
                Locality = countries.FirstOrDefault()?.Localities?.FirstOrDefault()
            };
    }
}
