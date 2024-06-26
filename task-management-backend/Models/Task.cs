﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace task_management_backend.Models;

[Table("tasks")]
public class Task : BaseEntity
{
    [Required] [Column("title")] public string Title { get; set; }

    [Column("description")] public string? Description { get; set; }

    [Column("due_date")]
    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; set; }

    [Required] [Column("status")] public string Status { get; set; }

    [Required]
    [Column("user_id")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    public User User { get; set; }
}