using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace task_management_backend.Models.DTOs;

public class TaskUpdateDto
{
    [Required] public string Title { get; set; }
    public string Description { get; set; }
    [JsonPropertyName("due_date")] public DateTime DueDate { get; set; }
    [Required] public string Status { get; set; }
}