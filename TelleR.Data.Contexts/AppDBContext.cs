using Microsoft.EntityFrameworkCore;
using TelleR.Data.Configuration;
using TelleR.Data.Entities;

namespace TelleR.Data.Contexts
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => new UserConfiguration(entity));
            builder.Entity<Blog>(entity => new BlogConfiguration(entity));
            builder.Entity<Post>(entity => new PostConfiguration(entity));
            builder.Entity<Comment>(entity => new CommentConfiguration(entity));
        }
    }
}
