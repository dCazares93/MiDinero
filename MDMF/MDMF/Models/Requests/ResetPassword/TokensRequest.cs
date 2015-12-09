using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests.ResetPassword
{
    public class TokensRequest
    {
        public string UserId { get; set; }
        public Guid Token { get; set; }
        public Int16 TokenTypeId { get; set; }
    }
}