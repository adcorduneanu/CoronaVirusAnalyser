namespace Corona.Domain.Entities
{
    using System.Collections.Generic;
    using Corona.Domain.ValueObjects;

    public class Locality
    {
        public Locality(string name, string country)
        {
            this.Name = name;
            this.Country = country;
        }

        public string Name { get; set; }
        public string Country { get; set; }
        public IReadOnlyList<DateValuePair> DateValues { get; private set; }

        public Locality WithDates(List<DateValuePair> dateValues)
        {
            this.DateValues = dateValues;

            return this;
        }
    }
}
