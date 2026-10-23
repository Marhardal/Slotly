using System;

namespace Slotly.Server;

public class BookingModel
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public Guid TenantID { get; set; }

    public Guid ServiceID { get; set; }

    public Guid statusID { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;

    public ServiceModel? Service { get; set; }

    public StatusModel? Status { get; set; }
}
