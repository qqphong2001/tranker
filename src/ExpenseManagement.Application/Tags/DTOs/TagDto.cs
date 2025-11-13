namespace ExpenseManagement.Application.Tags.DTOs;

public class TagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTagDto
{
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; } = "#6B7280"; // Default gray
}
