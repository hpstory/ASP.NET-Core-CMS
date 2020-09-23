using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Models.Comments
{
    public class CommentsAddDto
    {
        [Required(ErrorMessage = "请输入内容")]
        [MaxLength(500, ErrorMessage = "{0}长度不超过{1}个字！")]
        public string Content { get; set; }
        [Required(ErrorMessage = "请输入用户Id")]
        public string Author { get; set; }
        [Required(ErrorMessage = "请输入文章Id")]
        public int ArticleId { get; set; }
    }
}
