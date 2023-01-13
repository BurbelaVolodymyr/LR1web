using System;
using System.Collections.Generic;

namespace NewTravelAgency.Models;

public partial class Care
{
    public int Id { get; set; }

    public int? TicketId { get; set; }

    public string Name { get; set; } = null!;

    public int Cost { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
