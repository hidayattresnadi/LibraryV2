using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleLibraryAPI.Services;
using SimpleLibraryV2.Interfaces;
using SimpleLibraryV2.Models;
using SimpleLibraryV2.Services;

namespace SimpleLibraryV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BorrowingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BookManager _bookManager;
        public BorrowingController(IConfiguration configuration, BookManager bookManager)
        {
            _configuration = configuration;
            _bookManager = bookManager;
        }
        [HttpPost]
        public async Task<IActionResult> BorrowingBook([FromBody] BorrowingInput borrowingInput)
        {
            try
            {
                var durationLoanBooks = _configuration.GetValue<int>("BorrowedBooks:DurationLoanBooks");
                var maximumBorrowedBooks = _configuration.GetValue<int>("BorrowedBooks:MaximumBorrowedBooks");
                var inputBorrowing = await _bookManager.BorrowBook(borrowingInput, maximumBorrowedBooks, durationLoanBooks);
                return Ok(inputBorrowing);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("/Returning")]
        public async Task<IActionResult> ReturningBook([FromBody] BorrowingInput borrowingInput)
        {
            try
            {
                var inputBorrowing = await _bookManager.ReturnBook(borrowingInput);
                return Ok(inputBorrowing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
