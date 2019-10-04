using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string ExpireDate { get; set; }
    }
}