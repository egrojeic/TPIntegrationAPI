using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrdersConfirm
    {
        [Required]
        public int RegNumber { get; set; }
        [Required]
        public int OrderCode { get; set; }
        [Required]
        public string CodeBoxProduct { get; set; }
        public string BoxProduct { get; set; }
        [Required]
        public int QtyConfirm { get; set; }
        public string CancelReason { get; set; }
        public decimal UnitCostConfirm { get; set; }
    }
}