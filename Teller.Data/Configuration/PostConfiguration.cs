using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelleR.Data.Entities;

namespace TelleR.Data.Configuration
{
    public class PostConfiguration
    {
        public PostConfiguration(EntityTypeBuilder<Post> entity)
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Blog).WithMany(x => x.Posts);
            entity.HasOne(x => x.Author).WithMany(x => x.Posts);

            entity.Property(x => x.Title).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.PostContent).IsRequired();
            entity.Property(x => x.IsPublished).IsRequired();
        }
    }
}
