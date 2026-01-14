using System.ComponentModel.DataAnnotations;

namespace lLOCALS.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "يرجى إدخال كلمة المرور")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        // ميزة إضافية يمكنك تفعيلها لاحقاً
        public bool RememberMe { get; set; }
    }
}