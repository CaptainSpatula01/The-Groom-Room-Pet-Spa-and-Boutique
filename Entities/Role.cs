using Microsoft.AspNetCore.Identity;
namespace groomroom.Entities;

public class Role : IdentityRole<int>
{
    public List<UserRole> UserRoles { get; set; } = new();
}