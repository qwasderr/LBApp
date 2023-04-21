using System.ComponentModel.DataAnnotations;

namespace LBApp.ViewModel
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} має мати мінімум {2} та максимум {1} символів", MinimumLength = 5)]
        public string NewPassword { get; set; }
    }
}
