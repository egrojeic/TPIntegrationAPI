using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrderMaterialDetails
    {
        [JsonIgnore]
        public int ID { get; set; }
        public int MaterialCode { get; set; }
        public string Type { get; set; }
        public string Material { get; set; }
        public int Qty { get; set; }
    }
}