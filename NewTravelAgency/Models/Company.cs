using System;
using System.Collections.Generic;

namespace NewTravelAgency.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? WorkerNumber { get; set; }

    public virtual ICollection<Worker> Workers { get; } = new List<Worker>();
}
