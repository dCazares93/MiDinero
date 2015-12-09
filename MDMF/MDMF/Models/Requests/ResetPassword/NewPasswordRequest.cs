using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sabio.Web.Models.Requests.ResetPassword
{
    public class NewPasswordRequest
    {
        [Required]
        [RegularExpression("^.*(?=.{8,100})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).*$")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        public Guid ResetToken { get; set; }
    }
}