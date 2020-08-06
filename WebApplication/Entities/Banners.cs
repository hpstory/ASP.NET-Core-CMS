using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Entities
{
    public class Banners
    {
        [Key]
        public int ID { get; set; }
        public int Position { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public DateTime PublishTime { get; set; }
    }
}