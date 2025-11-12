using MDFusionLabHaute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{
    public class ExcelCanisterViewModel
    {
        public int? ID { get; set; }
        public int? UserID { get; set; }
        //public decimal? MaximumAmount { get; set; }
        //public decimal? MinimumAmount { get; set; }
        public decimal? CurrentAmount { get; set; }
        //public decimal? WarningAmount { get; set; }
        public string? CanisterCode { get; set; }
        public string? SKU { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        public string? Name { get; set; }
        //public int? UnitID { get; set; }
        //public string? UnitName { get; set; }
        public bool? IsActive { get; set; }
        //public bool? IsDeleted { get; set; }
    }
}