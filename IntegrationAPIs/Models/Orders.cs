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
        public int ID;
        public string PO;
        public int OrderCode;
        public string Type;
        public string FarmShipDate;
        public string MiamiShipDate;
        public string DeliveryDate;
        public string PullDate;
        public string Status;
        public int CustomerCode;
        public string Customer;
        public string Farm;
        public List<OrderDetails> Details;
    }
}