using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LBApp.Models;

public partial class Genre
{
    public int GenreId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва жанру")]
    public string GenreName { get; set; } = null!;
    
    [Display(Name = "Опис")]
    public string? GenreDescr { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
