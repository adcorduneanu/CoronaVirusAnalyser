namespace Corona.App.Models
{
    using System.Collections.Generic;

    public class CountryViewModel
    {
       public string Country { get; set; }
        public string Locality { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
