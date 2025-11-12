using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.Entities
{
    public class FormulaViewModel
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string? ProductCode { get; set; }
        public string? Collection { get; set; }
        public string? ProductName { get; set; }
        public string? ProductAmount { get; set; }
        public string? TotalRecord { get; set; }
        public decimal? DispenseAmount { get; set; } // Keep as decimal for calculations
        public string DispenseAmountString { get; set; } // For returning formatted amount
        public List<ColorAmountViewModel> ColorAmounts { get; set; } = new List<ColorAmountViewModel>();

        // New properties to hold color codes and amounts
        public string ColorCodes { get; set; }
        public string[] ColorCodesArray { get; set; } // For storing split values
        public string Amounts { get; set; }
        public string[] AmountsArray { get; set; } // For storing split values
    }
    public class ColorAmountViewModel
    {
        public string ColorCode { get; set; }
        public string Amount { get; set; }
    }
}
