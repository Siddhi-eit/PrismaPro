using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDFusionLabHaute.Domain.ViewModel
{

    public class ExcelDispenseViewModel
    {
        //public string AmountToDispense { get; set; }  
        public string AmountToDispensePerBottle { get; set; }

        public string ComponentNames { get; set; }

        //public string ProductCode { get; set; }
        public string TailoringCode { get; set; }

        public string ComponentAmounts { get; set; }

        public string LotNr { get; set; }

        public string DispensationsNumber { get; set; }

        //public int? ID { get; set; }
        //public string? /*MACHINE_REG_NO*/ { get; set; }
        //public string? COUNTRY { get; set; }
        //public string? SHOP { get; set; }
        //public DateTime? DATE { get; set; }
        //public string? TIME_ID_ENTERED { get; set; }
        //public string? CONSULTANT_ID { get; set; }
        //public string? DERMAPROFILE { get; set; }
        //public string? TAILORING_CODE { get; set; }
        //public string? PRICE { get; set; }
        //public string? TIME_DISPENSED { get; set; }
        //public string? BACH_LOT_NO { get; set; }

        //public string? CreatedDate { get; set; }
        public string CreatedDate { get; set; }
        
        //public string? ModifiedDate { get; set; }
        //public string? MachineID { get; set; }


    }
}