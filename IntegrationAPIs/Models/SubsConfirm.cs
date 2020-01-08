using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationAPIs.Models
{
    public class SubsConfirmResponse
    {
        public List<SubsConfirm> SubstitutionsConfirm { get; set; }
        public MsgResponse Response { get; set; }
    }

    public class SubsConfirm
    {
        public string PO;
        public int OrderCode;
        public string FarmShipDate;
        public string MiamiShipDate;
        public int CustomerCode;
        public string Customer;
        [Required]
        public int RegNumber;
        public int SeasonCode;
        public string Season;
        public int ModelCode;
        public string CodeBoxProduct;
        public string BoxProduct;
        public int Pack;
        public int BoxCode;
        public string Box;
        public int Qty;
        public int QtyConfirmed;
        [Required]
        public string Revised;
        [Required]
        public string Approved;
        public string SubstitutionFarm { get; set; }
        public string SubstitutionDone { get; set; }
    }

    public class SubsConfirmRequest
    {
        public List<SubsConfirm> SubstitutionsConfirm { get; set; }
    }
}