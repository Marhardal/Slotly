using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Slotly.Server;

public class BookingModel
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public Guid TenantID { get; set; }

    public Guid ServiceID { get; set; }

    public Guid statusID { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string? ClientName { get; set; }

    public string? ClientEmail { get; set; }

    public string? ClientPhone { get; set; }

    public string? StaffID { get; set; }

    public string? StaffName { get; set; }

    public int Duration { get; set; }

    public decimal Price { get; set; }

    public string? Notes { get; set; }

    [NotMapped]
    public List<Guid> AddOnIDs { get; set; } = [];

    public ICollection<ServiceAddOnsModel> AddOns { get; set; } = new List<ServiceAddOnsModel>();

    public DateTimeOffset createdOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset updatedOn { get; set; } = DateTimeOffset.UtcNow;

    [JsonIgnore]
    public ServiceModel? Service { get; set; }

    [JsonIgnore]
    public StatusModel? Status { get; set; }
}
