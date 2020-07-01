namespace Corona.App.Models
{
    using System.Collections.Generic;

    public class LocalityViewModel
    {
        public string CountryName { get; set; }
        public string LocalityName { get; set; }
        public List<DateValuePair> DateValues { get; set; }
    }
}
