using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.ViewModels
{
    public class UserLoginModel
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
