namespace Corona.App.Models
{
    using System;
    using System.Collections.Generic;
    using Corona.Domain.Entities;
    using Corona.Domain.ValueObjects;

    public sealed class LocalityViewModel
    {
        public string CountryName { get; private set; }
        public string LocalityName { get; private set; }
        public IEnumerable<DateValuePair> DateValues { get; private set; }

        internal static Func<Locality, LocalityViewModel> ProjectFromDomain = location =>
            new LocalityViewModel
            {
                CountryName = location.Country,
                LocalityName = location.Name,
                DateValues = location.DateValues
            };
    }
}
