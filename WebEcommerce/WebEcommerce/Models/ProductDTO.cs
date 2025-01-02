using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class ProductDTO
    {
        [Required]
        [MaxLength(100)]
        public string name { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string brand { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string category { get; set; } = "";
        [Required]
        [Precision(16, 2)]
        public decimal price { get; set; }
        [Required]
        public string description { get; set; } = "";
        public IFormFile? imageFileName { get; set; }
    }
}
