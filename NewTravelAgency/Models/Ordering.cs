using System;
using System.Collections.Generic;

namespace NewTravelAgency.Models;

public partial class Ordering
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int? WorkerId { get; set; }

    public int TicketNumber { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();

    public virtual Worker? Worker { get; set; }
}
