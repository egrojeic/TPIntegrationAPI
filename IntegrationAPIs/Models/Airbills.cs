using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class Airbills
    {
        [Required]
        public string AWB { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Airline { get; set; }
        public string Farm { get; set; }
        [Required]
        public string FarmInvoice { get; set; }
        public string HAWB { get; set; }
        public string ChargeAgency { get; set; }
        [Required]
        public List<AirbillDetails> Details { get; set; }
    }

    public class AirbillStatus
    {
        [Required]
        public string AWB { get; set; }
        [Required]
        public string Status { get; set; }
    }
}