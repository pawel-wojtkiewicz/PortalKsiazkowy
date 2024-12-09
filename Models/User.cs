using Microsoft.AspNetCore.Identity;

namespace PortalKsiazkowy.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; } // Na przykład, pełne imię
        public string PasswordHash { get; set; }
        public string Role { get; set; } // 'User' lub 'Admin'
        public List<Book> Library { get; set; }
    }
}
