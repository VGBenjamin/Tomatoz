using Microsoft.AspNetCore.Identity;
using Tomatoz.Shared.Enums;

namespace Tomatoz.Shared.Models;

// Forward declaration of ApplicationUser to avoid circular dependencies
// The actual implementation is in Tomatoz.Infrastructure.Identity
public interface IApplicationUser
{
    string Id { get; set; }
    string? UserName { get; set; }
    string? Email { get; set; }
    string? DisplayName { get; set; }
    UserRole Role { get; set; }
    int TotalPoints { get; set; }
    string? Bio { get; set; }
    string? Location { get; set; }
    string? Website { get; set; }
    DateTime JoinedAt { get; set; }
    DateTime? LastLoginAt { get; set; }
    bool EmailNotifications { get; set; }
    bool IsActive { get; set; }
}
