namespace MDFusionLabHaute.Domain.Entities
{
    public class Canisters : Audit
    {
        public int ID { get; set; }

        public int CanisterLookupId {  get; set; }
       /* public string Name { get; set; }*/
        public int? UserID { get; set; }
        public int? MachineID { get; set; }
/*        public string CanisterCode { get; set; }
        public string? SKU { get; set; }*/
        public decimal? MaximumAmount { get; set; }
        public decimal? CurrentAmount { get; set; }
        public decimal? MinimumAmount { get; set; }
        public decimal? WarningAmount { get; set; }
        public int? UnitID { get; set; }
        public int? IsActive { get; set; }
    }
}
