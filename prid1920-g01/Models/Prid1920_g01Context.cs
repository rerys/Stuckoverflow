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

            // modelBuilder.Entity<User>(entity => {
            //     entity.HasIndex(e => e.Pseudo).HasName("X_pseudo").IsUnique();
            // }
            // );

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