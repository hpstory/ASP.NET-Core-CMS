using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Models.Articles
{
    public class ArticlesDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public string PublishDate { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        // public string PublisherName { get; set; }
        public bool IsHot { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int CollectCount { get; set; }
    }
}
