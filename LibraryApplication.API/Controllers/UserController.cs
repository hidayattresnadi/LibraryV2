using Microsoft.AspNetCore.Mvc;
using SimpleLibraryV2.Interfaces;
using SimpleLibraryV2.Models;

namespace SimpleLibraryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            User factoryUser= Factory.GetUser(user);
            var inputUser = await _userService.AddUser(user);
            if (inputUser == null)
            {
                return BadRequest("User Name cannot be dupplicate");
            }
            return Ok(inputUser);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User user= await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser([FromBody] User user, int id)
        {
            var updatedUser = await _userService.UpdateUser(user, id);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool isDeletedUser = await _userService.DeleteUser(id);
            if (isDeletedUser == false)
            {
                return NotFound();
            }
            return Ok("User is deleted");
        }
    }
}
