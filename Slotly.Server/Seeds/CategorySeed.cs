namespace Slotly.Server.Seeds;

public static class CategorySeed
{
    // Static, hardcoded GUIDs and timestamps — required for HasData().
    // EF diffs seed rows by key on every migration; Guid.NewGuid() / DateTimeOffset.UtcNow
    // would make every row look changed and re-issue delete/insert for the whole table.
    private static readonly DateTimeOffset SeededOn =
        new(2026, 10, 22, 0, 0, 0, TimeSpan.Zero);

    public static List<CategoryModel> GetSeedData()
    {
        return
        [
            Row("c1a1b2c0-0001-4a10-9000-000000000001", "Massage & Bodywork", "Therapeutic massage, bodywork and aromatherapy treatments."),
            Row("c1a1b2c0-0001-4a10-9000-000000000002", "Facial Aesthetics", "Facials, skin treatments and aesthetic services."),
            Row("c1a1b2c0-0001-4a10-9000-000000000003", "Recovery", "Recovery sessions such as sauna, plunge and contrast therapy."),
            Row("c1a1b2c0-0001-4a10-9000-000000000004", "Holistic Ceremonies", "Couples rituals, sound baths and other holistic ceremonies."),
            Row("c1a1b2c0-0001-4a10-9000-000000000005", "Banquet & Gala Halls", "Banquet halls and gala venue bookings."),
            Row("c1a1b2c0-0001-4a10-9000-000000000006", "Hotel Luxury Stays", "Hotel rooms, suites and luxury stay bookings."),
            Row("c1a1b2c0-0001-4a10-9000-000000000007", "Corporate Venues", "Meeting rooms and corporate venue hire."),
            Row("c1a1b2c0-0001-4a10-9000-000000000008", "Weddings & Receptions", "Wedding, reception and celebration venue bookings."),
            Row("c1a1b2c0-0001-4a10-9000-000000000009", "Physical Therapy", "Physical therapy and rehabilitation appointments."),
            Row("c1a1b2c0-0001-4a10-9000-00000000000a", "Biomechanics", "Biomechanics assessments and related sessions."),
            Row("c1a1b2c0-0001-4a10-9000-00000000000b", "Therapeutic Modalities", "Clinical therapeutic modalities and treatment sessions."),
            Row("c1a1b2c0-0001-4a10-9000-00000000000c", "Architecture", "Architecture consultations and design appointments."),
            Row("c1a1b2c0-0001-4a10-9000-00000000000d", "Executive Advisory", "Executive advisory and professional consultation sessions.")
        ];
    }

    private static CategoryModel Row(string id, string name, string description) => new()
    {
        ID = Guid.Parse(id),
        Category = Categories.Services,
        Name = name,
        Description = description,
        createdOn = SeededOn,
        updatedOn = SeededOn
    };
}
