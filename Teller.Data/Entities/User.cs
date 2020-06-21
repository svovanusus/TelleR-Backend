using System;
using System.Collections.Generic;
using TelleR.Data.Enums;

namespace TelleR.Data.Entities
{
    public class User
    {
        public Int64 Id { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public String Username { get; set; }

        public String Password { get; set; }

        public Boolean IsActivate { get; set; }

        public Boolean IsBlocked { get; set; }

        public UserRole Role { get; set; }

        public String Avatar { get; set; }

        public virtual IEnumerable<Blog> Blogs { get; set; }

        public virtual List<BlogAuthor> AddedBlogs { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }

        public virtual IEnumerable<AuthorInvite> SendedInvites { get; set; }

        public virtual IEnumerable<AuthorInvite> ReceivedInvetes { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime? LastActive { get; set; }
    }
}
