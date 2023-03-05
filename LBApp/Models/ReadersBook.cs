using System;
using System.Collections.Generic;

namespace LBApp.Models;

public partial class ReadersBook
{
    public int Id { get; set; }

    public int ReaderId { get; set; }

    public int BookId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
