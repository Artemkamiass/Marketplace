using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 30 символов.")]
        public string Password {  get; set; } = string.Empty;


        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 30 символов.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; } = string.Empty;


        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [StringLength(30, MinimumLength = 12, ErrorMessage = "Длина строки должна быть от 12 до 30 символов.")]
        public string Email { get; set; } = string.Empty;
    }
}
