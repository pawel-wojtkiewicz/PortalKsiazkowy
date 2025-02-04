using System.ComponentModel.DataAnnotations;

namespace PortalKsiazkowy.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Autor jest wymagany.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Gatunek jest wymagany.")]
        public string Genre { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data publikacji jest wymagana.")]
        public DateTime PublishedDate { get; set; }

        public double AverageRating { get; set; }
        public int RatingsCount { get; set; }

        public string? ImageUrl { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}