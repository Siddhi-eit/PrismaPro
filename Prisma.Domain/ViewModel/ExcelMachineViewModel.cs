using MDFusionLabHaute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{
    public class ExcelMachineViewModel
    {
        public int? ID { get; set; }
        public string? MachineRegNo { get; set; }
        public string? ShopName { get; set; }
        public string? ShopAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? MacAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}