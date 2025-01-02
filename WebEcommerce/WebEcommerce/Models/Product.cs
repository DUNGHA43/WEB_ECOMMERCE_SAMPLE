using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class Product
    {
        public int id { get; set; }
        [MaxLength(100)]
        public string name { get; set; } = "";
        [MaxLength(100)]
        public string brand { get; set; } = "";
        [MaxLength(100)]
        public string category { get; set; } = "";
        [Precision(16,2)]
        public decimal price { get; set; }
        public string description { get; set; } = "";
        [MaxLength(100)]
        public string imageFileName { get; set; } = "";
        public DateTime createDate { get; set; }
    }
}
