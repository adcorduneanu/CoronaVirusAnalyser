namespace Corona.Infrastructure
{
    using System;

    public class Sick : Entity
    {
        public Sick()
        {
           this.Id = Guid.NewGuid();
        }

        public DateTime Date { get; set; }
        public string SourceUrl { get; set; }
        public int Records { get; set; }

        public virtual Locality Locality { get; set; }
    }
}