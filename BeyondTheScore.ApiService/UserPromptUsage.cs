using System.ComponentModel.DataAnnotations;

namespace BeyondTheScore.ApiService;

public class UserPromptUsage
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Auth0UserId { get; set; } = string.Empty;

    public int PromptCount { get; set; }

    public DateTime? LastPromptDate { get; set; }
}