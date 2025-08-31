namespace TNAI_Proj.Model.Entities
{
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MaintenanceRecord
{
    public int Id { get; set; }

    [Required]
    public int CarId { get; set; }
    public Car? Car { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime MaintenanceDate { get; set; }

    [Required]
    [StringLength(200)]
    public string? ServiceType { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public decimal Cost { get; set; }

    public int Mileage { get; set; }

    [StringLength(100)]
    public string? ServiceProvider { get; set; }

    public string Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
} 