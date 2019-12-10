using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class Orders
    {
        [JsonIgnore]
        public int ID { get; set; } 
        public string PO { get; set; }
        public int OrderCode { get; set; }
        public string Type { get; set; }
        public string FarmShipDate { get; set; }
        public string MiamiShipDate { get; set; }
        public string DeliveryDate { get; set; }
        public string PullDate { get; set; }
        public string Status { get; set; }
        public int CustomerCode { get; set; }
        public string Customer { get; set; }
        public string Farm { get; set; }
        public List<OrderDetails> Details { get; set; }
    }
}