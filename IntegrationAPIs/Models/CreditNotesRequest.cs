using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class CreditNotesRequest
    {
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int Code { get; set; }
    }

    public class CreditNotesStatusRequest
    {
        public List<CreditNotesStatus> Credits { get; set; }
    }
}