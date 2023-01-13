using System;
using System.Collections.Generic;

namespace NewTravelAgency.Models;

public partial class Worker
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? MiddleName { get; set; }

    public int CardId { get; set; }

    public int CodeId { get; set; }

    public int PhoneNumber { get; set; }

    public string? Address { get; set; }

    public int? Wage { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<Ordering> Orderings { get; } = new List<Ordering>();
}
