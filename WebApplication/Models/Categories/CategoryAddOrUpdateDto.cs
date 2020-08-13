using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Categories
{
    public class CategoryAddOrUpdateDto
    {
        [Display(Name = "类别")]
        [MaxLength(10, ErrorMessage = "{0}不能超过{1}个字符")]
        public string Name { get; set; }
    }
}
