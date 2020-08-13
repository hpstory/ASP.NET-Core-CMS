using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Comments
{
    public class CommentsAddDto
    {
        [Required(ErrorMessage = "请输入内容")]
        [MaxLength(500, ErrorMessage = "{0}长度不超过{1}个字！")]
        public string Content { get; set; }
        [Required(ErrorMessage = "请输入用户Id")]
        public Guid AuthorId { get; set; }
        [Required(ErrorMessage = "请输入文章Id")]
        public int ArticleId { get; set; }
    }
}
