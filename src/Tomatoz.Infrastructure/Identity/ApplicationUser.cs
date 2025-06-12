// This file now uses the ApplicationUser from Tomatoz.Shared.Models
// The actual ApplicationUser is defined in Tomatoz.Shared.Models.ApplicationUser

using Tomatoz.Shared.Models;

namespace Tomatoz.Infrastructure.Identity
{
    // This class is kept for backwards compatibility but inherits from the shared model
    public class ApplicationUser : Shared.Models.ApplicationUser
    {
    }
}
