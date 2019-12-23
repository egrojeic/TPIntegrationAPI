using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrderDetails
    {
        public int RegNumber;
        public int SeasonCode;
        public string Season;
        public int ModelCode;
        public string CodeBoxProduct;
        public string BoxProduct;
        public string BoxProductType;
        public int Pack;
        public int Qty;
        public int QtyConfirmed;
        public int Stems;
        public int BoxCode;
        public string Box;
        public decimal UnitPrice;
        public decimal UnitCost;
        public decimal TotalCost;
        public string PullDateWithFormat;
        public string UPC;
        public decimal UPCRetailPrice;
        public string TimeStampMaster;
        public string TimeStampRecipe;
        public string ReasonChange;
        public string Wet;
        public string Maritime;
        public int SemaphoreSubs;
        public List<OrderBunchDetails> Bunches;
        public List<OrderMaterialDetails> Materials;
    }
}