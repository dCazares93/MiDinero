using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sabio.Web.Models.Requests.Plans;
using System.ComponentModel.DataAnnotations;

namespace Sabio.Web.Models.Requests.Plans
{
    public class PlanFeatureUpdateRequest : PlanFeatureAddRequest
    {
        [Required]
        public int Id { get; set; }
    }
}