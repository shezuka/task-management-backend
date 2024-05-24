using System.ComponentModel.DataAnnotations;

namespace task_management_backend.Models.DTOs;

public class TaskCreateDto
{
    [Required] public string Title { get; set; }
    public string Description { get; set; }
    [Required] public DateTime DueDate { get; set; }
    [Required] public string Status { get; set; }
}