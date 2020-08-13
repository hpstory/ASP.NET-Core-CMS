using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Articles
{
    public class ArticlesAddOrUpdateDto
    {
        [Required(ErrorMessage = "请输入标题")]
        [MaxLength(100, ErrorMessage = "{0}不能超过{1}个字符")]
        public string Title { get; set; }
        [MaxLength(50, ErrorMessage = "{0}不能超过{1}个字符")]
        public string Author { get; set; }
        [Url(ErrorMessage = "图片链接已失效")]
        public string Cover { get; set; }
        [Required(ErrorMessage = "请选择时间")]
        public DateTime PublishDate { get; set; }
        [Required(ErrorMessage = "请输入正文")]
        public string Content { get; set; }
        [Required(ErrorMessage = "请选择分类")]
        public int CategoryID { get; set; }
    }
}
