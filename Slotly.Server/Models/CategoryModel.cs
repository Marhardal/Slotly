namespace Slotly.Server;

public class CategoryModel
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public Categories Category { get; set; }
  
    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<ServiceModel>? Services { get; set; }
}


public enum Categories
{
    Services
};
