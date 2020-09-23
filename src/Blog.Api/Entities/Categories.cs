using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Entities
{
    public class Categories
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "请输入类型")]
        public string Name { get; set; }
        public ICollection<Articles> Articles { get; set; }
    }
}
