using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class CancelationsConfirm
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
        public int QtyToCancel;
        [Required]
        public string Cancel;
    }

    public class CancelationsConfirmRequest
    {
        public List<CancelationsConfirm> Confirmations { get; set; }
    }
}