using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class BannersAddDto
    {
        [Display(Name = "顺序")]
        public int Position { get; set; }
        [Url(ErrorMessage = "图片已失效")]
        [Display(Name = "图片")]
        [Required(ErrorMessage = "请添加轮播图片")]
        public string ImageUrl { get; set; }
        [Url(ErrorMessage = "请输入正确的链接")]
        [Display(Name = "跳转链接")]
        public string Link { get; set; }
    }
}
