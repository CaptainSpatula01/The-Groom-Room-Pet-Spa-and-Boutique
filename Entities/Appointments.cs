using groomroom.Controllers;

namespace groomroom.Entities
{
    public class Appointments
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Reference { get; set; }
        public int Total { get; set; }
        public int UserId { get; set; }
        public Users Users { get; set; }
        public ICollection<Services> Services { get; set; }
    }
}
