using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Entities.Identity.Entities;

namespace WebApplication.Entities
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
        public User User { get; set; }
        [ForeignKey("CategoryID")]
        public Categories Category { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}