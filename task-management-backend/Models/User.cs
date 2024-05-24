using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_management_backend.Models;

[Table("users")]
public class User : BaseEntity
{
    [Required] [Column("username")] public string Username { get; set; }

    [Required] [Column("password_hash")] public string PasswordHash { get; set; }

    public List<Task> Tasks { get; set; }
}