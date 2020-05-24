using System;

namespace TelleR.Data.Entities
{
    public class AuthorInvite
    {
        public Int64 Id { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public virtual Blog Blog { get; set; }
        public Boolean? IsApprove { get; set; }
        public Boolean IsSenderNotified { get; set; } = false;
        public DateTime CreateDate { get; set; }
        public DateTime? ReceiverRespondDate { get; set; }
        public DateTime? SenderViewedDate { get; set; }
    }
}
