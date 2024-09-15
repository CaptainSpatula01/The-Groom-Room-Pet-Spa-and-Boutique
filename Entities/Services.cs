using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace groomroom.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
    }

    public class ServiceDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
    }

    public class ServiceEntityConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Services");
        }
    }

}
