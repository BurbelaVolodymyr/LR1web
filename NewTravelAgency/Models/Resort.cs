using System;
using System.Collections.Generic;

namespace NewTravelAgency.Models;

public partial class Resort
{
    public int Id { get; set; }

    public int? CountryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Country? Country { get; set; }

    public virtual ICollection<Hotel> Hotels { get; } = new List<Hotel>();
}
