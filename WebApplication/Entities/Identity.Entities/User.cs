using Microsoft.AspNetCore.Identity;
using System;

namespace WebApplication.Entities.Identity.Entities
{
    public class User : IdentityUser
    {
        public string NickName { get; set; }
        public string Gender { get; set; }
        public int GenderType { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
        public string Avatar { get; set; }
        public string Province { get; set; }
    }
}
