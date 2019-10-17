using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{

    public class AirbillRequest
    {
        [Required]
        public string Airbill { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Airline { get; set; }
        [Required]
        public string Farm { get; set; }
        [Required]
        public string FarmInvoice { get; set; }
        public string HAWB { get; set; }
        public string ChargeAgency { get; set; }
        [Required]
        public List<AirbillDetails> AirbillDetails { get; set; }
    }
}