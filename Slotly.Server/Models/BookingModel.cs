using System;

namespace Slotly.Server;

public class BookingModel
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public int MyProperty { get; set; }

}
