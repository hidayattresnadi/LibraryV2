using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleLibraryAPI.Services;
using SimpleLibraryV2.Interfaces;
using SimpleLibraryV2.Models;

namespace SimpleLibraryV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BorrowingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBorrowingService _borrowingService;
        public BorrowingController(IConfiguration configuration, IBorrowingService borrowingService)
        {
            _configuration = configuration;
            _borrowingService = borrowingService;
        }
        [HttpPost]
        public async Task<IActionResult> BorrowingBook([FromBody] BorrowingInput borrowingInput)
        {
            try
            {
                var durationLoanBooks = _configuration.GetValue<int>("BorrowedBooks:DurationLoanBooks");
                var maximumBorrowedBooks = _configuration.GetValue<int>("BorrowedBooks:MaximumBorrowedBooks");
                var inputBorrowing = await _borrowingService.BorrowBook(borrowingInput, maximumBorrowedBooks, durationLoanBooks);
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
                var inputBorrowing = await _borrowingService.ReturnBook(borrowingInput);
                return Ok(inputBorrowing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
