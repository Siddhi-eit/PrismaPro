namespace MDFusionLabHaute.Domain.Entities
{
    public class Users : Audit
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? RoleID { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        //public string? ProfileImage { get; set; }
        public bool? IsActive { get; set; }
        //public string? ResetCode { get; set; }
        //public DateTime? ResetCodeExpiry { get; set; }
        public string? MDFusionLabNo { get; set; }
        public string? BachLotNo { get; set; }
        public string? ConsultantID { get; set; }
        public string? Country { get; set; }
        public string? Shop { get; set; }
        public string? MacAddress { get; set; }
    }
}