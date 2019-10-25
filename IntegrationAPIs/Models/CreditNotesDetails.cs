using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class CreditNotesDetails
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string CodeBoxProduct { get; set; }
        public string BoxProduct { get; set; }
        public int Pack { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AdditionalCreditValue { get; set; }

    }
}