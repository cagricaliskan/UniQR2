using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.ViewModels.ApiDTO
{
    public class StudentRegisterModel
    {

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
