using System.ComponentModel.DataAnnotations;

namespace GanjAudio.WebApi.Model;
public class Note
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(500)]
    public string Content { get; set; } = "";

    [Required]
    public Guid OwnerId { get; set; }

    public bool IsPublic { get; set; } = false;
}