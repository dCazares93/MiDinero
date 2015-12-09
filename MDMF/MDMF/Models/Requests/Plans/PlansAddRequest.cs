using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Sabio.Web.Models.Requests.Tags;

namespace Sabio.Web.Models.Requests.Plans
{
    public class PlansAddRequest
    {
        private string _tag;

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MinLength(6)]
        public string Description { get; set; }

        [Required]
        public int Types { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public DateTime GoLiveDate { get; set; }

        [MinLength(3)]
        [MaxLength(100)]
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {

                if (value != null)
                {
                    _tag = value;
                }
            }
        }

        public int TagId { get; set; }
    }
}