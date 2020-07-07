namespace Corona.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public class Country: Entity
    {
        public Country()
        {
            this.Id = Guid.NewGuid();
            this.Localities = new List<Locality>();
        }

        public string Name { get; set; }

        public virtual List<Locality> Localities { get; set; }
    }
}