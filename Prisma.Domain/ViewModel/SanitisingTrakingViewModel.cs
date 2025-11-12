namespace MDFusionLabHaute.Domain.ViewModel
{
    public class SanitisingTrakingViewModel : AuditViewModel
    {
        public int ID { get; set; }
        public int? CanisterID { get; set; }
        public int? UserID { get; set; }
        public string FusionLabNo { get; set; }
        public string CanisterNo { get; set; }
        public int RefillingPeriod { get; set; }
        public DateTime? DateSanitised { get; set; }
        public int? ProductID { get; set; }
        public DateTime? SetReminder { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string CanisterCode { get; set; }
        public string Color { get; set; }
        public string ProductName { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public int? TotalRecord { get; set; }
        public bool IsSanitized { get; set; }
        public int? MachineID { get; set; }

    }
}
