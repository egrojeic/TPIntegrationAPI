using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Models
{
    public class OrderFlowerDetails
    {
        [JsonIgnore]
        public int ID ;
        public int TypeCode ;
        public string Type ;
        public string Category ;
        public int ColorCode ;
        public string Color ;
        public int FlowerCode ;
        public string Flower ;
        public int Qty ;
        public string Grade ;
        public int QualityCode ;
        public string Quality ;
        public int TreatmentCode ;
        public string Treatment ;
        public string BloomCount ;
        public string TreatmentTechnique ;
        public int TinctureTonesCode ;
        public string TinctureTones ;
        public string TinctureBase ;
        public string GlitterType ;

    }
}