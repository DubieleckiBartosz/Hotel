using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccountModels
{
    public class ResetPasswordRequest
    {
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
