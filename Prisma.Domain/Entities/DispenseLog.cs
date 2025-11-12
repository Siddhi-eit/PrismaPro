using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace MDFusionLabHaute.Domain.Entities
{
    public class DispenseLog : Audit
    {
        public int UserID { get; set; }
        public int MachineID { get; set; }
        public string ProductCode { get; set; }
        public decimal? AmountToDispense { get; set; }

        public int? DispensationsNumber { get; set; }
        public int? AmountToDispenseUnitId { get; set; }
        public int? ComponentUnitId { get; set; }
        public string ComponentNames { get; set; }
        public string ComponentAmounts { get; set; }
        public bool IsDispense { get; set; }
        public string MACHINE_REG_NO { get; set; }
        public string COUNTRY { get; set; }
        public string SHOP { get; set; }
        public DateTime DATE { get; set; }
        public DateTime TIME_ID_ENTERED { get; set; }
        public string CONSULTANT_ID { get; set; }
        public string DERMAPROFILE { get; set; }
        public string TAILORING_CODE { get; set; }
        public string ESSENCE { get; set; }
        public decimal PRICE { get; set; }
        public string BACH_LOT_NO { get; set; }
        public DateTime TIME_DISPENSED { get; set; }
        public string FormulaDispenseAmount { get; set; }
        public int DispanseQuantity { get; set; }

    }
}