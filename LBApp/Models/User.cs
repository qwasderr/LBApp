using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LBApp.Models

{
    public class User: IdentityUser
    {
        /*[Key]
        public int ReaderId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Iм'я користувача")]
        public string ReaderName { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

        public virtual ICollection<ReadersBook> ReadersBooks { get; } = new List<ReadersBook>();*/
    }
}
