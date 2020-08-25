using System;
using System.ComponentModel.DataAnnotations;
using WebApplication.Entities.Identity.Entities;

namespace WebApplication.Entities
{
    public class Comments
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "请输入内容")]
        public string Content { get; set; }
        [Required]
        public DateTime PublishTime { get; set; } = DateTime.Now;
        [Required]
        public int ArticleID { get; set; }
        public User User { get; set; }
        public Articles Articles { get; set; }
    }
}