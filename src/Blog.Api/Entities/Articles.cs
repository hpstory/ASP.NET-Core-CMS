using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Entities
{
    public class Articles
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public bool IsHot { get; set; } = false;
        //public User User { get; set; }
        [ForeignKey("CategoryID")]
        public Categories Category { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public int LikeCount { get; set; } = 0;
        public int DislikeCount { get; set; } = 0;
        public int CollectCount { get; set; } = 0;
    }
}
