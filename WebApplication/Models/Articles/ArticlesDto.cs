using System;

namespace WebApplication.Models.Articles
{
    public class ArticlesDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public DateTime ArticleDate { get; set; }
        public string Content { get; set; }
        public int CategoryID { get; set; }
        public Guid PublisherID { get; set; }
    }
}
