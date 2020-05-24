using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelleR.Data.Entities;

namespace TelleR.Data.Configuration
{
    public class BlogConfiguration
    {
        public BlogConfiguration(EntityTypeBuilder<Blog> entity)
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Posts).WithOne(x => x.Blog);
            entity.HasMany(x => x.AuthorInvites).WithOne(x => x.Blog);
            entity.HasMany(x => x.Authors).WithOne(x => x.Blog);

            entity.HasIndex(blog => blog.Name).IsUnique(true);

            entity.Property(x => x.Name).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(60).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(5000).IsRequired(false);
            entity.Property(x => x.Type).IsRequired();
            entity.Property(x => x.IsPublic).IsRequired();
            entity.Property(x => x.CreateDate).IsRequired();
            entity.Property(x => x.UpdateDate).IsRequired();
        }
    }
}
