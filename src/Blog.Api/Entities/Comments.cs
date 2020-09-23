using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Entities
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
        //public User User { get; set; }
        public Articles Articles { get; set; }
    }
}
