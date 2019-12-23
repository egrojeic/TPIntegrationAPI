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
        public int ID;
        public string Bunch;
        public int Qty;
        public int AssemblyTypeCode;
        public string AssemblyType;
        public string Length;
        public int Stems;
        public string UPCName;
        public List<OrderFlowerDetails> Flowers;
        public List<OrderMaterialDetails> Materials;
    }
}