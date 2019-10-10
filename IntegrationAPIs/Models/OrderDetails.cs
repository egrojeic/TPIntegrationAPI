using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrderDetails
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string CodeBoxProduct { get; set; }
        public string BoxProduct { get; set; }
        public int Pack { get; set; }
        public int Qty { get; set; }
        public int Stems { get; set; }
        public string Box { get; set; }
        public Decimal UnitCost { get; set; }
        public Decimal TotalCost { get; set; }
        public string PullDateWithFormat { get; set; }
        public string UPC { get; set; }
        public Decimal UPCRetailPrice { get; set; }
        public List<OrderBunchDetails> Bunches { get; set; }
    }
}