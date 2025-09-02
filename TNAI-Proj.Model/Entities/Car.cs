namespace TNAI_Proj.Model.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    public class Car
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Mileage { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Horsepower { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Torque { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TopSpeed { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Acceleration { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal FuelEfficiency { get; set; }

        [Required]
        public string? FuelType { get; set; }

        [Required]
        public string? Transmission { get; set; }

        public string? ImagePath { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? DealershipId { get; set; }
        public Dealership? Dealership { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<MaintenanceRecord>? MaintenanceRecords { get; set; }
    }
}