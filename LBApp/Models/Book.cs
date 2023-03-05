using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LBApp.Models;

public partial class Book
{
    public int BookId { get; set; }
    [Required(ErrorMessage ="Поле не повинно бути порожнім")]
    [Display(Name ="Назва книги")]
    public string BookName { get; set; } = null!;
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Рік написання")]
    public int BookYear { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ціна книги")]
    public double BookPrice { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва жанру")]
    public int GenreId { get; set; }
    
    [Display(Name = "Кількість сторінок")]
    public int? BookPagesCount { get; set; }
    
    [Display(Name = "Назва видавництва")]
    public int? PublishingHouseId { get; set; }

    public virtual ICollection<AuthorsBook> AuthorsBooks { get; } = new List<AuthorsBook>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();
    
    [Display(Name = "Назва жанру")]
    public virtual Genre Genre { get; set; } = null!;

    [Display(Name = "Назва видавництва")]
    public virtual PublishingHouse? PublishingHouse { get; set; }

    public virtual ICollection<ReadersBook> ReadersBooks { get; } = new List<ReadersBook>();
}
