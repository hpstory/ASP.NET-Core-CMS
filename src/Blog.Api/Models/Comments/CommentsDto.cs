using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Models.Comments
{
    public class CommentsDto
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public int ArticleId { get; set; }
        public string Avatar { get; set; }
    }
}
