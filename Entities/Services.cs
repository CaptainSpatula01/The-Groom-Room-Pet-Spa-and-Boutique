namespace groomroom.Entities
{
    public class Services
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int AppointmentId { get; set; }
        public Appointments Appointments { get; set; }
    }
}
