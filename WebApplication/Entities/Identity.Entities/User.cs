using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using WebApplication.Entities.Enum;

namespace WebApplication.Entities.Identity.Entities
{
    public class User : IdentityUser
    {
        public string NickName { get; set; }
        public string Gender { get; set; }
        public GenderType GenderType { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
        public string Avatar { get; set; }
        public string Province { get; set; }
        public ICollection<Articles> Articles { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}
