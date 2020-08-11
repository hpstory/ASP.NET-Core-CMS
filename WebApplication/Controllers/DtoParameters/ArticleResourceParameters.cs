using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers.DtoParameters
{
    public class ArticleResourceParameters
    {
        public const int MaxPageSize = 50;
        private int _pageSize = 10;
        public int? CategoryId { get; set; }
        public string SearchQuery { get; set; }
        public string OrderBy { get; set; } = "PublishDate";
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
    }
}
