using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrderDetails
    {
        public int RegNumber { get; set; }
        public int SeasonCode { get; set; }
        public string Season { get; set; }
        public int ModelCode { get; set; }
        public string CodeBoxProduct { get; set; }
        public string BoxProduct { get; set; }
        public int Pack { get; set; }
        public int Qty { get; set; }
        public int QtyConfirmed { get; set; }
        public int Stems { get; set; }
        public int BoxCode { get; set; }
        public string Box { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string PullDateWithFormat { get; set; }
        public string UPC { get; set; }
        public decimal UPCRetailPrice { get; set; }
        public string TimeStampMaster { get; set; }
        public string TimeStampRecipe { get; set; }
        public string ReasonChange { get; set; }
        public string Wet { get; set; }
        public string Maritime { get; set; }
        public List<OrderBunchDetails> Bunches { get; set; }
        public List<OrderMaterialDetails> Materials { get; set; }
    }
}