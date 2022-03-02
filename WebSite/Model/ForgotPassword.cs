using System.ComponentModel.DataAnnotations;

namespace WebSite.Model;

public class ForgotPassword
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public Guid UserToken { get; set; }

    public DateTime Expiration { get; set; }
}