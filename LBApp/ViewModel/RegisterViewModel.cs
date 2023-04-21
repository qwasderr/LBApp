using System.ComponentModel.DataAnnotations;
namespace LBApp.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="Нікнейм")]
        [DataType("string")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage ="Паролі не співпадають")]
        [Display(Name = "Підтвердження паролю")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
