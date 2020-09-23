using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Models.DtoParameters
{
    public class CommentResourceParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return 10; }
        }
        public string OrderBy { get; set; } = "Date";
        public int ArticleId { get; set; }
    }
}
