namespace Slotly.Server;

public class StatusModel
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public Category Category { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<BookingModel>? Bookings { get; set; }

}


    public enum Category
    {
        Payments,
        Bookings
    }
