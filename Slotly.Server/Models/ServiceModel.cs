using System;

namespace Company.ClassLibrary1;

public class ServiceModel
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public Guid CategoryID { get; set; }

    public Guid TenantID { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Duration { get; set; }

    public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;
}
