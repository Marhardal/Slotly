namespace Slotly.Server;

public class TenantModel
{
    public Guid ID { get; set; }

    public string? Name { get; set; }

    public string? Country { get; set; }

    public string? Currency { get; set; }

    public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<LocationsModel>? Locations { get; set; }
}


public class StaffsModel : EntityDefaults
{
    public Guid LocationID { get; set; }

    public string? UserID { get; set; }

    public Guid TenantID { get; set; }
}

