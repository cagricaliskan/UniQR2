using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;

namespace UniQR2.Controllers
{
    public class DownloadFileController : Controller
    {
        private readonly ModelContext db;

        public DownloadFileController(
                ModelContext db
            )
        {
            this.db = db;
        }

        // http://localhost:1453/DownloadFile/DownloadFile/?fileName=filename.ext
        //[Authorize("Instructor")] // zaman yokru hocam valla 22 saattir uyumuyorum 
        public IActionResult DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return NoContent();

            var fileFromDb = db.Files.FirstOrDefault(n => n.DataPath == fileName);
            // dosyaya erişim için gerekli ilişkisel sorgulamalar yapılacak
            // erişim izni yoksa ilgili http response dönülecek
            if (fileFromDb == null)
                return NoContent();

            Response.Headers.Clear();
            Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");

            string filePath = Path.Combine("UploadedFiles", fileFromDb.DataPath);
            string mimeType = MimeKit.MimeTypes.GetMimeType(fileName);

            var fileStream = new FileStream(filePath, FileMode.Open);

            return new FileStreamResult(fileStream, mimeType);
        }
    }
}
