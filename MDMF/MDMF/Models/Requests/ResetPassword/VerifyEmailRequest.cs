using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests.ResetPassword
{
    public class VerifyEmailRequest
    {
        [Required]
        [MaxLength(128)]
        public string Email { get; set; }
    }
}