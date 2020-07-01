namespace Corona.Infrastructure
{
    using Microsoft.EntityFrameworkCore;

    public class StorageContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("");
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Locality> Localities { get; set; }
        public DbSet<Sick> Sicks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
                .HasKey(x => x.Id); 
            modelBuilder.Entity<Country>()
                .HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Locality>()
                 .HasKey(x => x.Id); 
            modelBuilder.Entity<Sick>()
                .HasKey(x => x.Id);
        }
    }
}