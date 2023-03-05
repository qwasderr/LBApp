using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBApp.Models;

public partial class Author
{
    public int AuthorId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ім`я автора")]
    public string AuthorName { get; set; } = null!;
    [Display(Name = "Дата народження автора")]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime? AuthorDate { get; set; }
    [Display(Name = "Біографія")]
    public string? AuthorBiogr { get; set; }

    public virtual ICollection<AuthorsBook> AuthorsBooks { get; } = new List<AuthorsBook>();
}
