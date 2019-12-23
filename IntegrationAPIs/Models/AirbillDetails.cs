using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class AirbillDetails
    {
        [Required]
        public int OrderCode { get; set; }
        [Required]
        public int CustomerCode { get; set; }
        [Required]
        public string Customer { get; set; }
        [Required]
        public string CodeBoxProduct { get; set; }
        [Required]
        public string BoxedProduct { get; set; }
        [Required]
        public int BoxCode { get; set; }
        [Required]
        public string Box { get; set; }
        [Required]
        public int Pack { get; set; }
        [Required]
        public int Qty { get; set; }
        public int CompanyCode { get; set; }
        [Range(1,99999, ErrorMessage = "Length TrackBoxCode Invalid")]
        public int TrackBoxCode { get; set; }
        public decimal Cost { get; set; }
        public string Invoice { get; set; }
    }
}