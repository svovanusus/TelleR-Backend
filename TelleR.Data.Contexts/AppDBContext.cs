using Microsoft.EntityFrameworkCore;
using TelleR.Data.Configuration;
using TelleR.Data.Entities;

namespace TelleR.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<AuthorInvite> AuthorInvites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => new UserConfiguration(entity));
            builder.Entity<Blog>(entity => new BlogConfiguration(entity));
            builder.Entity<BlogAuthor>(entity => new BlogAuthorConfiguration(entity));
            builder.Entity<Post>(entity => new PostConfiguration(entity));
            builder.Entity<AuthorInvite>(entity => new AuthorInviteConfiguration(entity));
        }
    }
}
