using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers.DtoParameters
{
    public class CommentResourceParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return 10; }
        }
    }
}
