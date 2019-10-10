using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrderFlowerDetails
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string Flower { get; set; }
        public int Qty { get; set; }
        public string Grade { get; set; }
        public string Quality { get; set; }
        public string Treatment { get; set; }
        public string BloomCount { get; set; }
        public string TreatmentTechnique { get; set; }
        public string TinctureTones { get; set; }
        public string TinctureBase { get; set; }
        public string GlitterType { get; set; }

    }
}