using System;
using System.Collections.Generic;

namespace NewTravelAgency;

public partial class NewClient
{
    public int ClientId { get; set; }

    public string ClientName { get; set; } = null!;

    public string HotelName { get; set; } = null!;

    public string ResortName { get; set; } = null!;

    public DateTime? FirstTime { get; set; }

    public DateTime? SecondTime { get; set; }

    public int NumTicket { get; set; }
}
