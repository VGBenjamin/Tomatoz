using Microsoft.AspNetCore.Identity;
using Tomatoz.Shared.Enums;

namespace Tomatoz.Shared.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public UserRole Role { get; set; } = UserRole.Contributor;
    public int TotalPoints { get; set; } = 0;
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool EmailNotifications { get; set; } = true;
    public bool IsActive { get; set; } = true;
}
