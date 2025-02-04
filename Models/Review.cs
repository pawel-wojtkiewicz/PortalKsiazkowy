using PortalKsiazkowy.Models;
using System.ComponentModel.DataAnnotations;

namespace PortalKsiazkowy.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        [Required(ErrorMessage = "Treść recenzji nie może być pusta.")]
        [StringLength(1000, ErrorMessage = "Treść recenzji może mieć maksymalnie 1000 znaków.")]
        public string Content { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Ocena musi być w przedziale 1-6.")]
        public int Rating { get; set; }
    }
}