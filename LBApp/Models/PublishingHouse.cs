using System;
using System.Collections.Generic;

namespace LBApp.Models;

public partial class PublishingHouse
{
    public int PhId { get; set; }

    public string PhName { get; set; } = null!;

    public string? PhDescr { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
