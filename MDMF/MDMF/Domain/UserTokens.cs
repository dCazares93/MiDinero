using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Domain
{
    public class UserTokens
    {
        public Guid Token { get; set; }
        public Int16 TokenTypeId { get; set; }
        public string UserId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public string LanguageCode { get; set; }
    }
}