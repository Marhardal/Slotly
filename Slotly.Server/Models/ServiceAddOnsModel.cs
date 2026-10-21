namespace Slotly.Server;

public class ServiceAddOnsModel
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public Guid ServiceID { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;
}
