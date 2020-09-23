using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Models.Categories
{
    public class CategoryAddOrUpdateDto
    {
        [Display(Name = "类别")]
        [MaxLength(10, ErrorMessage = "{0}不能超过{1}个字符")]
        public string Name { get; set; }
    }
}
