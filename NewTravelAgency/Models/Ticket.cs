using System;
using System.Collections.Generic;

namespace NewTravelAgency.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int? OrderingId { get; set; }

    public int? HotelId { get; set; }

    public DateTime? TimeDeparture { get; set; }

    public DateTime? TimeArrival { get; set; }

    public int Cost { get; set; }

    public int TouristNumber { get; set; }

    public virtual ICollection<Care> Cares { get; } = new List<Care>();

    public virtual Hotel? Hotel { get; set; }

    public virtual Ordering? Ordering { get; set; }
}
