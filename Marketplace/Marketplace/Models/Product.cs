using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace Marketplace.Models
{
    public class Product
    {
            [Key]
            public long Id { get; set; }
            [Required]
            [StringLength(100)]
            public string Name { get; set; } = string.Empty;
            [Required]
            [StringLength(500)]
            public string Description { get; set; } = string.Empty;
            [Column(TypeName = "decimal(10, 2)")]
            [Required]
            public decimal Price { get; set; }
            [Required]
            public int Stock { get; set; }
            [ForeignKey("Category")]
            [Required]
            public long CategoryId { get; set; }
            public virtual Category Category { get; set; }
    }
}
