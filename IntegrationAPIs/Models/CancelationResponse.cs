using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class CancelationResponse
    {
        public List<Cancelations> CancelationsRequest { get; set; }
        public MsgResponse Response { get; set; }
    }
}