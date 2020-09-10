using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.UserInfo
{
    public class UserInfoDto
    {
        public string Avatar { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string Province { get; set; }
    }
}
