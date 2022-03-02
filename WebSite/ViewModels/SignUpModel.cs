using System.ComponentModel.DataAnnotations;

namespace GanjAudio.ViewModels;
public class SignUpModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربری الزامی است")]
    [MaxLength(20, ErrorMessage = "حداکثر ۲۰ کاراکتر مجاز است"), MinLength(3, ErrorMessage = "حداقل ۳ کاراکتر نیاز است")]
    [Display(Name = "نام کاربری")]
    [RegularExpression(@"^[a-zA-Z0-9]{3,20}$", ErrorMessage = "کاراکتر استفاده شده غیرمجاز است")]
    public string? UserName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور الزامی است")]
    [DataType(DataType.Password)]
    [MaxLength(30, ErrorMessage = "حداکثر ۳۰ کاراکتر مجاز است"), MinLength(8, ErrorMessage = "حداقل ۸ کاراکتر نیاز است")]
    [Display(Name = "رمز عبور (حداقل ۸ کاراکتر الزامی است)")]
    [RegularExpression(@"^[a-zA-Z0-9]{8,30}$", ErrorMessage = "کاراکتر استفاده شده غیرمجاز است")]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "تکرار رمزعبور اشتباه می‌باشد.")]
    [Display(Name = "تکرار رمز عبور")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "ایمیل الزامی است")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "آدرس ایمیل")]
    public string? Email { get; set; }

    [Compare("Email", ErrorMessage = "تکرار ایمیل اشتباه می‌باشد.")]
    [Display(Name = "تکرار آدرس ایمیل")]
    public string? ConfirmEmail { get; set; }
}