using groomroom.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace groomroom.Entities
{
    public class Pets
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Breed { get; set; }
        public int Size { get; set; }
        public int UserId { get; set; }
    }
    public class PetDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Breed { get; set; }
        public int Size { get; set; }
        public int OwnerId { get; set; }
    }

    public class PetEntityConfiguration : IEntityTypeConfiguration<Pets>
    {
        public void Configure(EntityTypeBuilder<Pets> builder)
        {
            builder.ToTable("Pets");
        }
    }
}
