using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public Guid PublisherID { get; set; }
        public Categories Category { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}