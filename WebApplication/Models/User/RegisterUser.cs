using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.User
{
    public class RegisterUser
    {
        [Required]
        public string NickName { get; set; }
        [Required]
        [MaxLength(16, ErrorMessage = "{0}不能超过{1}位")]
        public string Password { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
#nullable enable
        [EmailAddress]
        public string? Email { get; set; } = null;
    }
}
