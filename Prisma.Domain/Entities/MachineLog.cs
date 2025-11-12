namespace MDFusionLabHaute.Domain.Entities
{
    public class MachineLog : Audit
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public int? MachineID { get; set; }
        public string? Operation { get; set; }
        public string? OperationDetails { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
