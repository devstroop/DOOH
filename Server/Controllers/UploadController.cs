using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace DOOH.Server.Controllers
{
    public partial class UploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Single file upload
        [HttpPost("upload/single")]
        public IActionResult Single(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                var fileName = $"uploads/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

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

        // Multiple files upload
        [HttpPost("upload/multiple")]
        public IActionResult Multiple(IFormFile[] files)
        {
            try
            {
                if (files == null || files.Length == 0)
                {
                    return BadRequest("No files uploaded.");
                }

                List<string> fileUrls = new List<string>();
                foreach (var file in files)
                {
                    var fileName = $"uploads/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
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

        // Delete file
        [HttpDelete("upload/delete")]
        public IActionResult Delete(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("File name is required.");
                }

                var filePath = Path.Combine(_environment.WebRootPath, fileName);
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


        // Multiple files upload with parameter
        //[HttpPost("upload/{id}")]
        //public IActionResult Post(IFormFile[] files, int id)
        //{
        //    return Multiple(files);
        //}


        // Private method to save file
        private void SaveFile(IFormFile file, string fileName)
        {
            using (var stream = new FileStream(Path.Combine(_environment.WebRootPath, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
    }
}
