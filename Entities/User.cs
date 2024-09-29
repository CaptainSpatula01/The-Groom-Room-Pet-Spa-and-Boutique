//This is for users
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace groomroom.Entities;

public class User : IdentityUser<int>
{
    // public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<Pets> Pets { get; set; } = new();
    public List<Appointment> Appointments { get; set; } = new();
    public List<UserRole> UserRoles { get; set; } = new();
}
public class UserCreateDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<PetCreateDto> Pets { get; set; } = new();
    public List<int> RoleIds { get; set; } = new();
}

public class UserUpdateDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserGetDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<Pets>? Pets { get; set; }
    public List<AppointmentDto> Appointments { get; set; }
    public List<string> UserRoles { get; set; } = new();
}

public class PetCreateDto
{
    public string Name { get; set; }
    public string Breed { get; set; }
    public int Size { get; set; }
}

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName)
            .IsRequired();

        builder.Property(x => x.LastName)
            .IsRequired();

        builder.Property(x => x.UserName)
            .IsRequired();

        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.HasMany(u => u.Pets)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);
    }
}