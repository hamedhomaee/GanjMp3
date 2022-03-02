using System.ComponentModel.DataAnnotations;

namespace GanjAudio.ViewModels;

public class ResetPasswordViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور الزامی است")]
    [DataType(DataType.Password)]
    [MaxLength(30, ErrorMessage = "حداکثر ۳۰ کاراکتر مجاز است"), MinLength(8, ErrorMessage = "حداقل ۸ کاراکتر نیاز است")]
    [Display(Name = "رمز عبور جدید (حداقل ۸ کاراکتر الزامی است)")]
    [RegularExpression(@"^[a-zA-Z0-9]{8,30}$", ErrorMessage = "کاراکتر استفاده شده غیرمجاز است")]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "تکرار رمزعبور اشتباه می‌باشد.")]
    [Display(Name = "تکرار رمز عبور")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }

    [Required]
    public Guid Token { get; set; }

    [Required]
    public Guid UserId { get; set; }
}