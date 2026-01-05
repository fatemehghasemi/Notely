using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int NotesCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; } = string.Empty;
}

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; } = string.Empty;
}