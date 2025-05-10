using System.ComponentModel.DataAnnotations;

namespace BlindTypingTrainer.Web.ViewModels
{
    public class EditProfileVM
    {
        [Required]
        [Display(Name = "Логін")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Поточний пароль")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        [MinLength(6, ErrorMessage = "Мінімальна довжина пароля — 6 символів")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}
