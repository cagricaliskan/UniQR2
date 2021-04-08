using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class Files
    {

        [Key]
        public int FileID { get; set; }

        
        public string FileName { get; set; }
        
        
        public string FileType { get; set; }

        public byte[] DataFile { get; set; }
    }
}
