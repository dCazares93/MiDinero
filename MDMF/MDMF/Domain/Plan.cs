using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Domain
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public DateTime GoLiveDate { get; set; }
        public PlanType Type { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public List<PlanFeature> PlanFeature { get; set; }
        public string Tag { get; set; }
        public int TagId { get; set; }
        public string LanguageCode { get; set; }
    }
}