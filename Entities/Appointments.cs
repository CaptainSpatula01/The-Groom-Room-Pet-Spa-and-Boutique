namespace groomroom.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<int> ServiceId { get; set; }
        public decimal Total { get; set; } 
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Service> Service { get; set; }
    }

    public class AppointmentCreateDto
    {
        public string Date { get; set; }
        public List<int> ServiceId { get; set; }
    }

    public class AppointmentDto
    {
        public string Date { get; set; } // "MM-DD-YYYY"
        public List<int> ServiceId { get; set; }
        public List<string> ServiceDescription { get; set; }
        public decimal Total { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
    }

}
