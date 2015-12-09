using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Domain
{
    public class Referral
    {
        public int Id { get; set; }
        public int PartnerType { get; set; }
        public string PartnerName { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public string AffiliateCode { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public string LanguageCode { get; set; }
    }
}