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
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set;}
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Vote>().HasKey(v => new { v.Post.PostId, v.User.Id});

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(true);

            modelBuilder
            .Entity<User>()
            .HasIndex(u => u.Pseudo)
            .IsUnique(true);

            modelBuilder
                .Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique(true);
        }
    }
}