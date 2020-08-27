using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Upload
{
    [ApiController]
    [Route("upload")]
    public class FileUploadController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;

        public FileUploadController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(IFormFile formFile)
        {
            var currentDate = DateTime.Now;
            var webRootPath = _hostEnvironment.ContentRootPath;

            try
            {
                var filePath = $"/UploadFile/{currentDate:yyyyMMdd}/";
                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }

                if (formFile != null)
                {
                    var fileExtension = Path.GetExtension(formFile.FileName);

                    var fileSize = formFile.Length;

                    if (fileSize > 2 * 1024 *1024)
                    {
                        return Forbid();
                    }

                    var saveName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf('.')) + "_" + currentDate.ToString("HHmmss") + fileExtension;

                    using var fileStream = System.IO.File.Create(webRootPath + filePath + saveName);
                    await formFile.CopyToAsync(fileStream);

                    var completeFilePath = Path.Combine(filePath, saveName);
                    return Ok(new { completeFilePath });
                }

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
