namespace groomroom.Entities
{
    public class Pets
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public int Size { get; set; }
        public int UserId { get; set; }
    }
    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public int Size { get; set; }
        public int OwnerId { get; set; }
    }
}
