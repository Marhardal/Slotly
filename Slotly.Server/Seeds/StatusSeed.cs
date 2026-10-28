namespace Slotly.Server.Seeds;

public static class StatusSeed
{
    private static readonly DateTimeOffset SeededOn =
        new(2026, 10, 28, 0, 0, 0, TimeSpan.Zero);

    public static List<StatusModel> GetSeedData()
    {
        return
        [
            Row("d1a1b2c0-0001-4a10-9000-000000000001", "Pending", "The booking is awaiting confirmation."),
            Row("d1a1b2c0-0001-4a10-9000-000000000002", "Confirmed", "The booking has been confirmed."),
            Row("d1a1b2c0-0001-4a10-9000-000000000003", "Completed", "The booking has been completed."),
            Row("d1a1b2c0-0001-4a10-9000-000000000004", "Cancelled", "The booking has been cancelled.")
        ];
    }

    public static Guid PendingId => Guid.Parse("d1a1b2c0-0001-4a10-9000-000000000001");

    private static StatusModel Row(string id, string name, string description) => new()
    {
        ID = Guid.Parse(id),
        Category = Category.Bookings,
        Name = name,
        Description = description,
        createdOn = SeededOn,
        updatedOn = SeededOn
    };
}
