using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrdersResponse
    {
        public List<Orders> Orders { get; set; }
        public MsgResponse Message { get; set; }
    }
}