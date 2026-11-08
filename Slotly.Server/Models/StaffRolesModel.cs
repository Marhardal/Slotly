using Microsoft.AspNetCore.Identity;

namespace Slotly.Server;

// public class RolesModel : IdentityRole
// {
//     public Guid TenantID { get; set; }

//     public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;

//     public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;
// }

public class StaffRolesModel : IdentityUserRole<Guid>
{
    public Guid LocationID { get; set; }

    public Guid TenantID { get; set; }
}