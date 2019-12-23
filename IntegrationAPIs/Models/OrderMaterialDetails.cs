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
        public int ID ;
        public int MaterialCode ;
        public string Type ;
        public string Material ;
        public int Qty ;
    }
}