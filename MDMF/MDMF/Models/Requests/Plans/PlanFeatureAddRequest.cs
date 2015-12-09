using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests.Plans
{
    public class PlanFeatureAddRequest
    {
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Required]
        public int PlanId { get; set; }

    }
}