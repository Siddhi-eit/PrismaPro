namespace MDFusionLabHaute.Domain.Entities
{
    public class SanitisingTraking : Audit
    {
        public int ID { get; set; }
        public int? CanisterID { get; set; }
        public int? UserID { get; set; }
        public string FusionLabNo { get; set; }
        //public string CanisterNo { get; set; }
        public DateTime? DateSanitised { get; set; }
        //public int? ProductID { get; set; }
        public DateTime? SetReminder { get; set; }
        public bool? IsActive { get; set; }
        public int? MachineID { get; set; }
        
    }
}
