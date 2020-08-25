using System;

namespace WebApplication.Models.Articles
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
        public string PublisherName { get; set; }
        public bool IsHot { get; set; }
    }
}
