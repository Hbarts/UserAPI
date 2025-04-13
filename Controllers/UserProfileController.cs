using Microsoft.AspNetCore.Mvc;
using UserProfileAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace UserProfileAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private static List<UserProfile> _users = new List<UserProfile>
        {
            new UserProfile { Id = 1, Username = "admin", Password = "password", Email = "admin@example.com", FullName = "Admin User" },
            new UserProfile { Id = 2, Username = "john", Password = "1234", Email = "john@example.com", FullName = "John Doe" }
        };

        private static string? _loggedInUser = null;

        // GET: api/UserProfile
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_users);
        }

        // GET: api/UserProfile/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        // POST: api/UserProfile
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserProfile newUser)
        {
            newUser.Id = _users.Max(u => u.Id) + 1;
            _users.Add(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // PUT: api/UserProfile/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserProfile updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("User not found.");

            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.Email = updatedUser.Email;
            user.FullName = updatedUser.FullName;

            return NoContent();
        }

        // DELETE: api/UserProfile/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("User not found.");

            _users.Remove(user);
            return NoContent();
        }

        // POST: api/UserProfile/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserProfile loginUser)
        {
            var user = _users.FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);
            if (user == null) return Unauthorized("Invalid username or password.");

            _loggedInUser = user.Username;
            return Ok($"User '{_loggedInUser}' logged in successfully.");
        }

        // POST: api/UserProfile/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (_loggedInUser == null) return BadRequest("No user is logged in.");

            var user = _loggedInUser;
            _loggedInUser = null;
            return Ok($"User '{user}' logged out successfully.");
        }
    }
}