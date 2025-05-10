using System.ComponentModel.DataAnnotations;

namespace BlindTypingTrainer.Web.ViewModels
{
    public class RegisterVM
    {
        [Required] public string UserName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, DataType(DataType.Password)] public string Password { get; set; }
        [Required, Compare("Password"), DataType(DataType.Password)] public string ConfirmPassword { get; set; }
    }
}
