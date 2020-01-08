using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class Substitutions
    {
        [JsonIgnore]
        public int ID;
        public string PO;
        public int CustomerOrderCode;
        [Required]
        public int OrderCode;
        public string Type;
        public string FarmShipDate;
        public string MiamiShipDate;
        public string DeliveryDate;
        public int CustomerCode;
        public string Customer;
        public string Farm;
        public List<SubstitutionsDetails> Details;
    }

    public class SubstitutionsRequest
    {
        public List<Substitutions> Substitutions { get; set; }
    }
}