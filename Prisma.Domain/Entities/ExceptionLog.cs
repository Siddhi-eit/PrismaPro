namespace MDFusionLabHaute.Domain.Entities
{
    public class ExceptionLog
    {
        public int ID { get; set; }
        public string ExceptionSource { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Createdby { get; set; }

    }
}
