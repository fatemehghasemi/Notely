using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class TagDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int NotesCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTagDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; } = string.Empty;
}

public class UpdateTagDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; } = string.Empty;
}
