using Microsoft.EntityFrameworkCore;



namespace prid1920_g01.Models
{
    public class Prid1920_g01Context : DbContext
    {
        public Prid1920_g01Context(DbContextOptions<Prid1920_g01Context> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { Id=10,Pseudo = "Benito", Password = "Ben", Email = "Ben@epfc.eu", LastName = "Penelle", FirstName = "Ben", Reputation = 1 },
                new User { Id=11,Pseudo = "Bruno", Password = "Bruno", Email = "Bruno@epfc.eu", LastName = "Lacroix", FirstName = "Bru", Reputation = 1 }
            );

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(true);

            modelBuilder
            .Entity<User>()
            .HasIndex(u => u.Pseudo)
            .IsUnique(true);
        }
    }
}