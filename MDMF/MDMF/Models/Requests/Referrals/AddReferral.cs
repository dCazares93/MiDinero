using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests.Referrals
{
    public class AddReferral
    {
        [Required] //this is going to be required field
        public int PartnerType { get; set; }

        [Required] //this is going to be required field
        public string PartnerName { get; set; }

        [Required] //this is going to be required field
        public string Description { get; set; }

        [Required] // this is going to be a required field
        public Uri URL { get; set; }

        [Required] // this is going to be a required field
        public string AffiliateCode { get; set; }
    }
}