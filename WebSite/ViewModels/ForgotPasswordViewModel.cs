using System.ComponentModel.DataAnnotations;

namespace GanjAudio.ViewModels;
public class ForgotPasswordViewModel
{
    [Required]
    [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل نامعتبر است")]
    public string? Email { get; set; }
}