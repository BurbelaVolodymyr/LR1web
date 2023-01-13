using System;
using System.Collections.Generic;

namespace NewTravelAgency.Models;

public partial class Hotel
{
    public int Id { get; set; }

    public int? ResortId { get; set; }

    public string Name { get; set; } = null!;

    public int StarsNumber { get; set; }

    public string Room { get; set; } = null!;

    public bool? Safe { get; set; }

    public bool? Conditioner { get; set; }

    public bool? WiFi { get; set; }

    public string Bed { get; set; } = null!;

    public bool? MiniBar { get; set; }

    public virtual Resort? Resort { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
