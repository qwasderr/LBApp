using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LBApp.Models;

public partial class AuthorsBook
{
    public int Id { get; set; }
    [Display(Name = "Ім'я автора")]
    public int AuthorId { get; set; }
    [Display(Name = "Назва книги")]
    public int BookId { get; set; }
    [Display(Name = "Ім'я автора")]
    public virtual Author Author { get; set; } = null!;
    [Display(Name = "Назва книги")]
    public virtual Book Book { get; set; } = null!;
}
