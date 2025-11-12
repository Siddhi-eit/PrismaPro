namespace MDFusionLabHaute.Domain.ViewModel
{
    public class MachineViewModel : AuditViewModel
    {
        public int ID { get; set; }
        public string? MachineRegNo { get; set; }
        public string? ShopName { get; set; }
        public string? ShopAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? MacAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IsActive { get; set; }
        public int? TotalRecord { get; set; }
    }
}