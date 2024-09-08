namespace groomroom.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class ServiceDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
