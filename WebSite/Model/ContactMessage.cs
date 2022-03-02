using System.ComponentModel.DataAnnotations;

namespace WebSite.Model;

public class ContactMessage
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "ایمیل الزامی است")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "آدرس ایمیل")]
    public string? Email { get; set; }

    [Display(Name = "پیغام شما")]
    public string? Message { get; set; }
}