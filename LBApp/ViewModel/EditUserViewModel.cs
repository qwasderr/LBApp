using System.ComponentModel.DataAnnotations;
namespace LBApp.ViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Нікнейм - обов'язкове поле")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email - обов'язкове поле")]
        [Display(Name = "Email")]
        public string Email2 { get; set; }
        public int Year { get; set; }
    }
}
