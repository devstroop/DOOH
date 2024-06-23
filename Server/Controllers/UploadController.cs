using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace DOOH.Server.Controllers
{
    [Authorize]
    public partial class UploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload/single")]
        public IActionResult Single(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                var user = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = user?.Value;
                var subDirectory = $"uploads/{userId}";
                var contentType = file.ContentType;
                if (contentType.Contains("image/"))
                {
                    subDirectory += "/images";
                }
                else if (contentType.Contains("video/"))
                {
                    subDirectory += "/videos";
                }
                
                // var fileName = $"uploads/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fileName = $"{subDirectory}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                
                using (var stream = new FileStream(Path.Combine(_environment.WebRootPath, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var url = Url.Content($"/{fileName}");
                return Ok(new { Url = url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("upload/multiple")]
        public IActionResult Multiple(IFormFile[] files)
        {
            try
            {
                if (files == null || files.Length == 0)
                {
                    return BadRequest("No files uploaded.");
                }

                var user = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = user?.Value;
                var subDirectory = $"uploads/{userId}";
                List<string> fileUrls = new List<string>();
                foreach (var file in files)
                {
                    var contentType = file.ContentType;
                    if (contentType.Contains("image/"))
                    {
                        subDirectory += "/images";
                    }
                    else if (contentType.Contains("video/"))
                    {
                        subDirectory += "/videos";
                    }
                    var fileName = $"{subDirectory}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    SaveFile(file, fileName);
                    fileUrls.Add(Url.Content($"/{fileName}"));
                }

                return Ok(new { Urls = fileUrls });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("upload/delete/{**key}")]
        public IActionResult Delete(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return BadRequest("File name is required.");
                }
                key = key.Replace("/", "\\");
                var filePath = Path.Combine(_environment.WebRootPath, key);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        private void SaveFile(IFormFile file, string fileName)
        {
            var fullFileName = Path.Combine(_environment.WebRootPath, fileName);
            if (System.IO.File.Exists(fullFileName))
            {
                throw new Exception("File already exists.");
            }
            var directory = Path.GetDirectoryName(fullFileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
            using var stream = new FileStream(Path.Combine(_environment.WebRootPath, fileName), FileMode.Create);
            file.CopyTo(stream);
        }
    }
}
