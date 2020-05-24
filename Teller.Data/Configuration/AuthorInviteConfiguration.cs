using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelleR.Data.Entities;

namespace TelleR.Data.Configuration
{
    public class AuthorInviteConfiguration
    {
        public AuthorInviteConfiguration(EntityTypeBuilder<AuthorInvite> entity)
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Sender).WithMany(x => x.SendedInvites).IsRequired().OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Receiver).WithMany(x => x.ReceivedInvetes).IsRequired().OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Blog).WithMany(x => x.AuthorInvites).IsRequired().OnDelete(DeleteBehavior.NoAction);

            entity.Property(x => x.IsApprove).IsRequired(false);
            entity.Property(x => x.IsSenderNotified).IsRequired();
            entity.Property(x => x.CreateDate).IsRequired();
            entity.Property(x => x.ReceiverRespondDate).IsRequired(false);
            entity.Property(x => x.SenderViewedDate).IsRequired(false);
        }
    }
}
