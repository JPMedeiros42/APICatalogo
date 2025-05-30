﻿using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO;

public class RegisterModel
{
    [Required(ErrorMessage = "Username is required.")]
    public string? Username { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }
}
