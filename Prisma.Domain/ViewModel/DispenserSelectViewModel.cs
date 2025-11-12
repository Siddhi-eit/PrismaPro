using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{
    public class DispenserSelectViewModel
    {
        public string? AmountToDispense { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? ComponentUnit { get; set; }
        public string? ComponentNames { get; set; }
        public string? ComponentAmounts { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? TotalRecord { get; set; }
        public int ID { get; set; }

        public int DispensationsNumber {  get; set; }

        public string? BachLotNo {  get; set; }
        public bool IsDispense { get; set; }
    }
}
