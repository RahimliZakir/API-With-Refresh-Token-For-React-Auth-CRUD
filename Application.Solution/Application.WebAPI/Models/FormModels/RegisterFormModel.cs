using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Application.WebAPI.Models.FormModels
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "İstifadəçi adı göndərilməyib!")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Elektron poçt ünvanı göndərilməyib!")]
        [EmailAddress(ErrorMessage = "Elektron poçt düzgün formatda göndərilməyib!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifrə göndərilməyib!")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Şifrə göndərilməyib!")]
        [Compare("Password", ErrorMessage = "Şifrələr bir-birinə bərabər olmalıdır!")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
