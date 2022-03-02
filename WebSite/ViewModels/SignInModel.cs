using System.ComponentModel.DataAnnotations;

namespace GanjAudio.ViewModels;
public class SignInModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربری یا ایمیل الزامی است")]
    [Display(Name = "نام کاربری یا ایمیل")]
    public string? NameOrEmail { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور الزامی است")]
    [DataType(DataType.Password)]
    [MaxLength(30, ErrorMessage = "حداکثر ۳۰ کاراکتر مجاز است"), MinLength(8, ErrorMessage = "حداقل ۸ کاراکتر نیاز است")]
    [Display(Name = "رمز عبور")]
    public string? Password { get; set; }

    [Display(Name = "مرا به خاطر بسپار")]
    public bool IsPersistent { get; set; } = false;
}