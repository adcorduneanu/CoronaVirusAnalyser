namespace Corona.Domain.Entities
{
    using System.Collections.Generic;

    public class Country
    {
        public string Name { get; set; }
        public List<string> Localities { get; set; }
    }
}
