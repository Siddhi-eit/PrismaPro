using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{
    public class ProductWithFormulaViewModel
    {
        public int ID { get; set; }
        public string? ProductCode { get; set; }
        public int? UnitID { get; set; }
        public decimal TotalFormulaPriceUSD { get; set; }
        public decimal TotalFormulaPriceEURO { get; set; }
        public decimal TotalFormulaPriceMXN { get; set; }
        public decimal TotalFormulaPriceGBP { get; set; }
        public string? Essence1_number_of_drops { get; set; }
        public string? Essence2_number_of_drops { get; set; }
        public string? Essence3_number_of_drops { get; set; }
        public string? Essence4_number_of_drops { get; set; }
        public string? ColorCode { get; set; }
        public string? Amount { get; set; }
        public string? MDFusionLabNo { get; set; }
        public string? BachLotNo { get; set; }
        public decimal? dispenseAmount { get; set; }
        
        public string? Addon_A { get; set; }

        public string? Addon_B { get; set; }

        public string? Addon_C { get; set; }

        public string? Addon_D { get; set; }

        public string? Ingredients { get; set; }
    }
}