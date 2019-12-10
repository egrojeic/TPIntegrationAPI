using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrderBunchDetails
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string Bunch { get; set; }
        public int Qty { get; set; }
        public int AssemblyTypeCode { get; set; }
        public string AssemblyType { get; set; }
        public string Length { get; set; }
        public int Stems { get; set; }
        public string UPCName { get; set; }
        public List<OrderFlowerDetails> Flowers { get; set; }
        public List<OrderMaterialDetails> Materials { get; set; }
    }
}