using System.ComponentModel.DataAnnotations;

namespace WebSite.Model;

public class SiteUser
{
    public Guid Id { get; set; }

    public string? UserName { get; set; } = "";

    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}