using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{
    public class DispenserViewModel
    {
        public int UserID { get; set; }
        public double AmountToDispense { get; set; }
        public long AmountToDispenseUnitID { get; set; }
        public long ComponentUnitID { get; set; }
        public string[] ComponentNames { get; set; }
        public string[] ComponentAmounts { get; set; }
    }
}