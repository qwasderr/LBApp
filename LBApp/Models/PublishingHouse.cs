using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LBApp.Models;

public partial class PublishingHouse
{
    [Key]
    public int PhId { get; set; }
    [Display(Name = "Назва видавництва")]
    public string PhName { get; set; } = null!;
    [Display(Name = "Опис видавництва")]
    public string? PhDescr { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
