namespace MDFusionLabHaute.Domain.ViewModel
{
    public class RefillTrackingViewModel : AuditViewModel
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public int? MachineID { get; set; }
        public int? CanisterID { get; set; }
        public string FusionLabNo { get; set; }
        public DateTime? DateFilled { get; set; }
        public int? RefillML { get; set; }
        public decimal? Quantity { get; set; }
        public int? ProductID { get; set; }
        public int? LotNr { get; set; }
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CanisterCode { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int? TotalRecord { get; set; }
        public int? UnitID { get; set; }
        public bool IsActive { get; set; }
        public bool IsRefilled { get; set; }
    }

}