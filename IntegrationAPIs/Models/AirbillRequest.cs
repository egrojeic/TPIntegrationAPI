using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{

    public class AirbillRequest
    {
        public List<Airbills> Airbills { get; set; }
    }

    public class AirbillStatusRequest
    {
        public List<AirbillStatus> Airbills { get; set; }
    }
}