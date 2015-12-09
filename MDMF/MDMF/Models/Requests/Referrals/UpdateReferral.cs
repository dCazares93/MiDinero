using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests.Referrals
{
    public class UpdateReferral : AddReferral
    {
        [Required]
        public int Id { get; set; }
    }
}