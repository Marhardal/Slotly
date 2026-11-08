using Microsoft.AspNetCore.Identity;

namespace Slotly.Server;

public class LocationsModel
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public Guid TenantID { get; set; }

    public string? Location { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Coordinates { get; set; }

    public TimeOnly Open { get; set; } = TimeOnly.FromTimeSpan(new TimeSpan(08, 00, 00));

    public TimeOnly Close { get; set; } = TimeOnly.FromTimeSpan(new TimeSpan(17, 00, 00));

    public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;

    public TenantModel? Tenant { get; set; }

    public ICollection<LocationHoursModel>? Hours { get; set; }

    public ICollection<LocationStaffsModel>? Staffs { get; set; }
}

public class LocationHoursModel : EntityDefaults
{
    public Guid LocationID { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeOnly Open { get; set; } = new(8, 0);
    public TimeOnly Close { get; set; } = new(17, 0);

    public LocationsModel? Locations { get; set; }
}


public class LocationStaffsModel : EntityDefaults
{
    public Guid LocationID { get; set; }

    public Guid TenantID { get; set; }

    public string? UserID { get; set; }

    public LocationsModel? Locations { get; set; }

    public IdentityUser? User { get; set; }
}



public class EntityDefaults
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedOn { get; set; } = DateTimeOffset.UtcNow;
}