using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



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
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbQuery<ScoredPost> ScoredPosts { get; set; }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vote>().HasKey(v => new { v.PostId, v.UserId });
            modelBuilder.Entity<PostTag>().HasKey(p => new { p.PostId, p.TagId });

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

            // Post.Parent (1) <--> Post.Responses (*)
            modelBuilder.Entity<Post>()
                .HasOne(c => c.Parent)
                .WithMany(p => p.Responses)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment.Post (1) <--> Post.Comments (*)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment.User (1) <--> Post.Comments (*)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post.User (1) <--> User.Posts (*)
            modelBuilder.Entity<Post>()
                .HasOne(P => P.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post.accepted
            modelBuilder.Entity<Post>()
                .HasOne(P => P.Accpeted)
                .WithMany()
                .HasForeignKey(p => p.AcceptedAnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vote.post (1) <--> Post.votes (*)
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vote.user (1) <--> User.votes (*)
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // PostTag.post (1) <--> Post.PostTag (*)
            modelBuilder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // PostTag.Tag (1) <--> Tag.PostTag (*)
            modelBuilder.Entity<PostTag>()
                .HasOne(t => t.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(t => t.TagId)
                .OnDelete(DeleteBehavior.Restrict);




        }
    }
}