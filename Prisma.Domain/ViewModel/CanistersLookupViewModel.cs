namespace MDFusionLabHaute.Domain.ViewModel
{
    public class CanistersLookupViewModel : AuditViewModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? CanisterCode { get; set; }
        public string? SKU { get; set; }
        public int IsActive { get; set; }
        public int? TotalRecord { get; set; }
    }
}
