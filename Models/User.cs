using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; }

    [StringLength(200)]
    public string Address { get; set; }

    [Required]
    public string Role { get; set; } = "User";

    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Car> BoughtCars { get; set; }
}