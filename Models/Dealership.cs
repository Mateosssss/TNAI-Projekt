namespace TNAI_Proj.Models
{
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Dealership
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    [Required]
    [StringLength(200)]
    public string? Address { get; set; }

    [Required]
    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? ManagerName { get; set; }

    public string? OperatingHours { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Car>? Cars { get; set; }
}
} 