using MDFusionLabHaute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{
    public class ExcelRefillTrackingViewModel
    {
        public int? ID { get; set; }
        //public int? UserID { get; set; }
        //public decimal? MachineID { get; set; }
        //public decimal? CanisterID { get; set; }
        public decimal? FusionLabNo { get; set; }
        public int? LotNr { get; set; } 
        public DateTime? DateFilled { get; set; }
        public string? Quantity { get; set; }
        //public string? ProductID { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }
        //public string? Email { get; set; }
        //public string? UserName { get; set; }
        public string? CanisterCode { get; set; }
        public string? Name { get; set; }
        //public string?  SKU { get; set; }
        //public string? ProductName { get; set; }
        public bool? IsActive { get; set; }
        //public bool? IsDeleted { get; set; }
    }
}