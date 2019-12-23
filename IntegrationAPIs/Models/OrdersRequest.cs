using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrdersRequest
    {
        [DataType(DataType.Date)]
        public DateTime InitialDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime FinalDate { get; set; }
        public int OrderCode { get; set; }
        public string PendingOrders { get; set; }
    }

    public class OrdersConfirmRequest
    {
        public List<OrdersConfirm> Confirmations { get; set; }
    }

}