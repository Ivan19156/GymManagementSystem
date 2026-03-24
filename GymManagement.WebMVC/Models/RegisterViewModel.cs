using System.ComponentModel.DataAnnotations;

namespace GymManagement.WebMVC.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Невірний формат Email")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Пароль обов'язковий")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має містити щонайменше 6 символів")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Паролі не збігаються")]
    public string ConfirmPassword { get; set; } = "";
}
