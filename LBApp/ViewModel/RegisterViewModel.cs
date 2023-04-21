using System.ComponentModel.DataAnnotations;
namespace LBApp.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name ="Нікнейм")]
        [DataType("string")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email2 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} має мати мінімум {2} та максимум {1} символів", MinimumLength = 5)]
        public string Password { get; set; }
        [Required(ErrorMessage ="Обов'язкове поле")]
        [Compare("Password", ErrorMessage ="Паролі не співпадають")]
        [Display(Name = "Підтвердження паролю")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
