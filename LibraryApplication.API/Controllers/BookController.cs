using Microsoft.AspNetCore.Mvc;
using SimpleLibraryV2.Interfaces;
using SimpleLibraryV2.Models;

namespace SimpleLibraryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            Book bookFactory = Factory.GetBook(book);
            var inputBook = await _bookService.AddBook(bookFactory);
            if (inputBook == null)
            {
                return BadRequest("Book title cannot be dupplicate");
            }
            return Ok(inputBook);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            Book book = await _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBook([FromBody] Book book, int id)
        {
            var updatedBook = await _bookService.UpdateBook(book, id);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return Ok(updatedBook);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            bool isDeletedBook = await _bookService.DeleteBook(id);
            if (isDeletedBook == false)
            {
                return NotFound();
            }
            return Ok("Book is deleted");
        }
    }
}
