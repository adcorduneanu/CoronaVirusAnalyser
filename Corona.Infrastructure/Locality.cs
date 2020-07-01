namespace Corona.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public class Locality: Entity
    {
        public Locality()
        {
            this.Id = Guid.NewGuid();
            this.Sicks = new List<Sick>();
        }

        public string Name { get; set; }

        public virtual Country Country { get; set; }
        public virtual List<Sick> Sicks { get; set; }
    }
}