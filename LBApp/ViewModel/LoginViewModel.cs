using System.ComponentModel.DataAnnotations;
namespace LBApp.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Нікнейм")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Обов'язкове поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Запам`ятати")]
public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
