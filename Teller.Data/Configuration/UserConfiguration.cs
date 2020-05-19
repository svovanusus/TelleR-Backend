using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelleR.Data.Entities;

namespace TelleR.Data.Configuration
{
    public class UserConfiguration
    {
        public UserConfiguration(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Blogs).WithOne(x => x.Owner);
            entity.HasMany(x => x.Posts).WithOne(x => x.Author);
            entity.HasMany(x => x.Comments).WithOne(x => x.Author);

            entity.HasIndex(x => x.Username).IsUnique();
            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.FirstName).HasMaxLength(30).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Username).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Password).HasMaxLength(64).IsRequired();
            entity.Property(x => x.IsActivate).IsRequired();
            entity.Property(x => x.IsBlocked).IsRequired();
            entity.Property(x => x.Role).IsRequired();
        }
    }
}
