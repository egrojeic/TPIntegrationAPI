using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class CreditNotes
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string TypeDoc { get; set; }
        public int Code { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Airbill { get; set; }
        public decimal TotalValue { get; set; }
        public List<CreditNotesDetails> Details { get; set; }
    }

    public class CreditNotesStatus
    {
        [Required]
        public int Code { get; set; }
        [Required]
        public string Status { get; set; }
    }
}