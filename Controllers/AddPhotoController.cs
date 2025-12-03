using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace WebApplication1.Controllers
{
    [Route("api/[Controller]")]
    public class AddPhotoController : ControllerBase
    {
        [HttpPost("AddPhoto")]

        public async Task<IActionResult>
            PostPhoto(IFormFile file)
        {
            if (file.Length == 0)
                return BadRequest("Файл не получен!");

            var uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{file.FileName}";
            uniqueFileName = uniqueFileName.Replace(" ", "");
            var path = Path.Combine(Directory.GetCurrentDirectory(),"DataSource", "Documents", uniqueFileName);
            if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "DataSource", "Documents")))
                System.IO.Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "DataSource", "Documents"));

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(uniqueFileName);

        }

        [HttpPut("EditPhoto/{PathPhoto}")]
        public async Task<IActionResult>
            PutPhoto(IFormFile file, string PathPhoto)
        {
            if (file.Length == 0)
                return BadRequest("Файл не получен!");
            if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "DataSource", "Documents")))
                System.IO.Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "DataSource", "Documents"));
            var path = Path.Combine(Directory.GetCurrentDirectory(), "DataSource", "Documents", PathPhoto);
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
            {
                PathPhoto = $"{DateTime.Now:yyyyMMddHHmmssfff}_{file.FileName}";
                PathPhoto= PathPhoto.Replace(" ", "");
                path = Path.Combine(Directory.GetCurrentDirectory(), "DataSource", "Documents", PathPhoto);
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(PathPhoto);

        }

        [HttpGet("Photo/{filePath}")]
        public async Task<IActionResult>
            GetPhoto(string filePath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "DataSource", "Documents", filePath);
            if (!System.IO.File.Exists(path))
                return NotFound("Фтогогафия не найдена!");
            try
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                return File(memory, MimeHelper.GetMimeType(filePath));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        
        public class MimeHelper()
        {
            public static string GetMimeType(string path)
            {
                var contentTypeProvider = new FileExtensionContentTypeProvider();
                if (contentTypeProvider.TryGetContentType(path, out var mimeType))
                {
                     return mimeType;
                }
                return "application/octet-stream";
            }
        }
    }
}
