namespace groomroom.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public List<int> ServiceId { get; set; }
        public decimal Total { get; set; } 
    }

    public class AppointmentDto
    {
        public int Id { get; set; }
        public string Date { get; set; } // "MM-DD-YYYY"
        public int UserId { get; set; }
        public List<int> ServiceId { get; set; }
        public decimal Total { get; set; }
    }

}
