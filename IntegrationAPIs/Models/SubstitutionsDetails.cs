using System.ComponentModel.DataAnnotations;

namespace IntegrationAPIs.Models
{
    public class SubstitutionsDetails
    {
        [Required]
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
        public string Wet;
        public string Maritime;
        public int SemaphoreSubs;
        [Required]
        public string Notes { get; set; }
    }
}