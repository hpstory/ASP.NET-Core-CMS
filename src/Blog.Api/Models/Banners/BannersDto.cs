using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Models.Banners
{
    public class BannersDto
    {
        public int ID { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
    }
}
