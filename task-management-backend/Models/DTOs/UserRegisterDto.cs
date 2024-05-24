﻿using System.ComponentModel.DataAnnotations;

namespace task_management_backend.Models.DTOs;

public class UserRegisterDto
{
    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }
}