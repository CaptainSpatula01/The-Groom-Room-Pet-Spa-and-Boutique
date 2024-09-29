using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        public AdminController(UserManager<User> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("users")]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("users/{Id}")]
        public async Task<IActionResult> GetUserById(int Id)
        {
            var user = await _userManager.Users
                .Include(u => u.Pets)
                .FirstOrDefaultAsync(u => u.Id == Id);
            
            if (user == null) return NotFound(new { message = "User Not Found"});

            var result = new
            {
                user.Id,
                user.UserName,
                user.Email,
                Pets = user.Pets.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Breed,
                    p.Size
                })
            };

            return Ok(user);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto createUserDto)
        {
            var user = new User
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (result.Succeeded) return Ok(new { message = "User created successfully" });
            return BadRequest(result.Errors);
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            user.Email = updateUserDto.Email;
            user.UserName = updateUserDto.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(new { message = "User updated successfully" });
            return BadRequest(result.Errors);
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new {message = "User Not Found"});
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return Ok(new {message = $"User {id} deleted successfully"});
            return BadRequest(result.Errors);
        }

        [HttpGet("appointments")]
        public async Task<IActionResult> ListAppointments()
        {
            var appointments = await _context.Appointments.ToListAsync();
            return Ok(appointments);
        }

        [HttpGet("appointments/{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if(appointment == null)
            {
                return NotFound(new {message = "Appointment Not Found"});
            }
            return Ok(appointment);
        }

        [HttpDelete("appointments/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if(appointment == null)
            {
                return NotFound(new {message = "Appointment Not Found"});
            }
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return Ok(new {message = $"Appointment {id} deleted successfully"});
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboardData()
        {
            var dashboardData = new 
            {
                TotalUsers = _userManager.Users.Count(),
                TotalAppointments = _context.Appointments.Count(),
                Timestamp = DateTime.Now
            };
            return Ok(dashboardData);
        }
    }
}