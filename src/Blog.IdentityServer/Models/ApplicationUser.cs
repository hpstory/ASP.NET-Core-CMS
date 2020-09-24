using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Blog.IdentityServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {
        public string LoginName { get; set; }

        public string RealName { get; set; }

        public int Gender { get; set; } = 0;

        public int Age { get; set; }

        public DateTime Birth { get; set; } = DateTime.Now;

        public string Address { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
