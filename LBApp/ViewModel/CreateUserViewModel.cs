using System.ComponentModel.DataAnnotations;

namespace LBApp.ViewModel
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Нікнейм - обов'язкове поле")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email - обов'язкове поле")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email2 { get; set; }
        [Required(ErrorMessage = "Пароль - обов'язкове поле")]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} має мати мінімум {2} та максимум {1} символів", MinimumLength = 5)]
        public string Password { get; set; }
        public int Year { get; set; }
    }
}
