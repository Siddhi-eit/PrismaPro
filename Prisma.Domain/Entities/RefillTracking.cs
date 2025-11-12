namespace MDFusionLabHaute.Domain.Entities
{
    public class RefillTracking : Audit
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public int? MachineID { get; set; }
        public int? CanisterID { get; set; }
        public string FusionLabNo { get; set; }
        public DateTime? DateFilled { get; set; }
        public decimal? Quantity { get; set; }
        public int? LotNr { get; set; }
        public int? UnitID { get; set; }
        public bool IsActive { get; set; }
    }
}
