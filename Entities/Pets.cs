namespace groomroom.Entities
{
    public class Pets
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Size { get; set; }
        public int UserId { get; set; }
        public Users Users { get; set; }
    }
}
