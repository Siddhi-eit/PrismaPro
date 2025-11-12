using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{
    public class DesktopAlertViewModel
    {

        public List<RefillAlertViewModel> refillAlertViewModelList { get; set; }
        public List<sanitizationAlertViewModel> sanitizationAlertViewModelList { get; set; }

        public DesktopAlertViewModel()
        {
            refillAlertViewModelList = new List<RefillAlertViewModel>();
            sanitizationAlertViewModelList = new List<sanitizationAlertViewModel>();
        }
    }

    public class RefillAlertViewModel
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public string CanisterCode { get; set; }
        public string Color { get; set; }
        public decimal? CurrentAmount { get; set; }
        public decimal? MinimumAmount { get; set; }
        public decimal? WarningAmount { get; set; }
    }

    public class sanitizationAlertViewModel
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public string CanisterCode { get; set; }
        public string Color { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime DateSanitised { get; set; }

    }
}