using Microsoft.AspNetCore.Identity;

namespace WebEcommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string fisrtName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string createDate { get; set; }
    }
}
