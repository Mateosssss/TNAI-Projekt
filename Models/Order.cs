namespace TNAI_Proj.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int CarId { get; set; }
        public Car? Car { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string? Status { get; set; } // Pending, Completed, Cancelled, Refunded

        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? CompletionDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
} 