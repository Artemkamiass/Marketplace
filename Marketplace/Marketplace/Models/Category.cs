using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
