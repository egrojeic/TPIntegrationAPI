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
        public int TrackBoxCode { get; set; }
        public int CustomerCode { get; set; }
        public string CodeBoxProduct { get; set; }
        public int SeasonCode { get; set; }
        public int ModelCode { get; set; }
        public string BoxProduct { get; set; }
        public string Reason { get; set; }
        public int Pack { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AdditionalCreditValue { get; set; }
        public string Invoice { get; set; }
        public int CompanyCode { get; set; }
    }
}