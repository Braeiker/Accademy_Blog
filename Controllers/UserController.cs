using Blog.Dto;
using Blog.Service;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // Restituisci tutti gli utenti
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Restituisci utente per nome
        [HttpGet("{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _userService.GetUserByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Creare un nuovo utente
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto newUserDto)
        {
            if (newUserDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            var result = await _userService.CreateUserAsync(newUserDto);
            if (result)
            {
                return CreatedAtAction(nameof(GetUserByName), new { name = newUserDto.Name }, newUserDto);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
        }

        // Aggiornare un utente esistente
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateUser(string name, [FromBody] UserDto updatedUserDto)
        {
            if (updatedUserDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            var result = await _userService.UpdateUserAsync(name, updatedUserDto);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        // Eliminare un utente esistente
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteUser(string name)
        {
            var result = await _userService.DeleteUserAsync(name);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        // Restituisci tutti gli utenti con post e commenti
        [HttpGet("details")]
        public async Task<IActionResult> GetAllUsersWithDetails()
        {
            var users = await _userService.GetAllUsersWithDetailsAsync();
            return Ok(users);
        }

        // Restituisci utente per nome con post e commenti
        [HttpGet("details/{name}")]
        public async Task<IActionResult> GetUserByNameWithDetails(string name)
        {
            var user = await _userService.GetUserByNameWithDetailsAsync(name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
