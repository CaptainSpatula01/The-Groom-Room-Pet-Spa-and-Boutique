using Microsoft.AspNetCore.Identity;

namespace groomroom.Entities;

public class Role : IdentityRole<int>
{
    public List<UserRole> Users { get; set; } = new();
}