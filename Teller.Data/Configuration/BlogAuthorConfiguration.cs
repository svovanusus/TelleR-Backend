using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelleR.Data.Entities;

namespace TelleR.Data.Configuration
{
    public class BlogAuthorConfiguration
    {
        public BlogAuthorConfiguration(EntityTypeBuilder<BlogAuthor> entity)
        {
            entity.HasKey(x => new { x.BlogId, x.AuthorId });

            entity.HasOne(x => x.Blog).WithMany(x => x.Authors);
            entity.HasOne(x => x.Author).WithMany(x => x.AddedBlogs);
        }
    }
}
