using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class CreditNotesResponse
    {
        public List<CreditNotes> CreditNotes { get; set; }
        public MsgResponse Response { get; set; }
    }
}