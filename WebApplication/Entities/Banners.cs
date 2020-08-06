using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Infrastructure.Services;

namespace WebApplication.Entities
{
    public class Banners
    {
        [Key]
        public int ID { get; set; }
        public int Position { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PublishTime { get; set; }
    }
}