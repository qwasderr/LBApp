using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBApp.Models;

public partial class Comment
{
    [Key]
    public int ComId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Текст коментаря")]
    public string ComText { get; set; } = null!;
    [Display(Name = "Дата та час написання коментаря")]
    public DateTime ComDate { get; set; }
    [Display(Name = "Назва книги")]
    public int BookId { get; set; }
    [Display(Name = "Ім'я читача")]
    public int ReaderId { get; set; }
    [Display(Name = "Назва книги")]
    public virtual Book Book { get; set; } = null!;
    [Display(Name = "Ім'я читача")]
    public virtual Reader Reader { get; set; } = null!;
}
