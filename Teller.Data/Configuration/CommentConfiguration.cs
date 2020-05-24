using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelleR.Data.Entities;

namespace TelleR.Data.Configuration
{
    public class CommentConfiguration
    {
        public CommentConfiguration(EntityTypeBuilder<Comment> entity)
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Author).WithMany(x => x.Comments);
            entity.HasOne(x => x.Post).WithMany(x => x.Comments);

            entity.Property(x => x.Text).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.CreateDate).IsRequired();
        }
    }
}
