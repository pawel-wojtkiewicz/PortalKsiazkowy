using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalKsiazkowy.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PortalKsiazkowy.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public BookController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Book
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 6)
        {
            var booksQuery = _context.Books.AsQueryable();

            ViewBag.SearchString = searchString; // Przekazujemy frazę wyszukiwania do widoku

            if (!string.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            var totalBooks = await booksQuery.CountAsync(); // Liczba książek
            var books = await booksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            return View(books);
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            // Sprawdzenie, czy użytkownik jest adminem
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                // Jeśli nie jest adminem, przekierowanie np. na stronę główną
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Genre,PublishedDate,ImageUrl")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.AverageRating = 0;  // Ustawienie domyślnej średniej oceny
                book.RatingsCount = 0;   // Liczba ocen na początku
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,PublishedDate,AverageRating,RatingsCount,ImageUrl")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        // Funkcja do oceny książki
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int id, [Range(1, 6, ErrorMessage = "Ocena musi być w przedziale 1-6.")] int rating)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var review = new Review
            {
                BookId = id,
                UserId = user.Id,
                Rating = rating,
                Content = ""
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var bookReviews = await _context.Reviews.Where(r => r.BookId == id).ToListAsync();
            book.AverageRating = bookReviews.Average(r => r.Rating);
            book.RatingsCount = bookReviews.Count;

            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        // Rekomendacje książek
        public async Task<IActionResult> Recommendations()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Pobierz zalogowanego użytkownika
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Pobierz książki, które użytkownik już ocenił
            var userReviews = await _context.Reviews
                .Where(r => r.UserId == user.Id)
                .Include(r => r.Book)
                .ToListAsync();

            // Jeżeli użytkownik nie ocenił żadnej książki
            if (!userReviews.Any())
            {
                return View(new List<Book>());
            }

            // Zbierz gatunki i autorów książek, które użytkownik ocenił
            var ratedGenres = userReviews.Select(r => r.Book.Genre).Distinct().ToList();
            var ratedAuthors = userReviews.Select(r => r.Book.Author).Distinct().ToList();

            // Znajdź książki, które pasują do ocenianych przez użytkownika (na podstawie gatunku lub autora)
            var recommendedBooks = await _context.Books
                .Where(b => ratedGenres.Contains(b.Genre) || ratedAuthors.Contains(b.Author))
                .ToListAsync(); // Załadowanie danych do pamięci

            // Filtruj książki, które użytkownik już ocenił
            recommendedBooks = recommendedBooks.Where(b => !userReviews.Any(r => r.BookId == b.Id)).ToList();

            // Ogranicz do 5 rekomendacji
            recommendedBooks = recommendedBooks.Take(5).ToList();

            return View(recommendedBooks);
        }


    }
}
