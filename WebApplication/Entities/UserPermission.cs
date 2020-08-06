using System.ComponentModel.DataAnnotations;

namespace WebApplication.Entities
{
    public class UserPermission
    {
        [Key]
        public int ID { get; set; }
        public string PermissionName { get; set; }
    }
}