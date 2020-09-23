using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Models
{
    public class FileUploadDto
    {
        [Required]
        public string FileName { get; set; }
        [Required, Url]
        public string FilePath { get; set; }
    }
}
