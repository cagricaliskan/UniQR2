using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Middlewares.Attributes;
using UniQR2.Models;

namespace UniQR2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileServerController : ControllerBase
    {
        private readonly ModelContext db;

        public FileServerController(
                ModelContext db
            )
        {
            this.db = db;
        }

        // http://localhost:1453/api/FileServer/fileName.ext
        [HttpGet("{fileName}")]
        [JWTAuthorize]
        public IActionResult GetFileAsync(string fileName)
        {
            var fileFromDb = db.Files.FirstOrDefault(n => n.DataPath == fileName);
            // dosyaya erişim için gerekli ilişkisel sorgulamalar yapılacak
            // erişim izni yoksa ilgili http response dönülecek
            if (fileFromDb == null)
                return NoContent();

            string filePath = Path.Combine("UploadedFiles", fileFromDb.DataPath);
            string mimeType = MimeKit.MimeTypes.GetMimeType(fileName);

            var fileStream = new FileStream(filePath, FileMode.Open);

            return new FileStreamResult(fileStream, mimeType);

        }
    }
}
