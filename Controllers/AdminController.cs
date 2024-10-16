using groomroom.Common;
using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public async Task<IActionResult> ListUsers()
        {
            var response = new Response();

            var users = await _context.Users
                .Include(u => u.Pets)
                .Include(u => u.Appointments)
                .ToListAsync();
            
            var userDtos = new List<UserGetDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);               
                var userGetDto = new UserGetDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Pets = user.Pets.Select(p => new Pets
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Breed = p.Breed,
                        Size = p.Size,
                        UserId = p.UserId
                    }).ToList(),
                    Appointments = user.Appointments.Select(a => new AppointmentDto
                    {
                        Date = a.Date.ToString("MM-dd-yyyy"),
                        ServiceId = a.ServiceId,
                        ServiceDescription = _context.Services 
                            .Where(s => a.ServiceId.Contains(s.Id))
                            .Select(s => s.Description)
                            .ToList(),
                        Total = a.Total,
                    }).ToList(),
                    UserRoles = roles.ToList() 
                };

                userDtos.Add(userGetDto);
            }

            response.Data = userDtos;

            return Ok(response);
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
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .ToListAsync();

            var appointmentDtos = appointments.Select(a => new AppointmentDto
            {
                Date = a.Date.ToString("MM-dd-yyyy"),
                ServiceId = a.Service.Select(s => s.Id).ToList(),
                ServiceDescription = a.Service.Select(s => s.Description).ToList(),
                Total = a.Total,
                UserId = a.User.Id,
                UserName = a.User.FirstName + " " + a.User.LastName,
                UserEmail = a.User.Email
            }).ToList();

            return Ok(appointmentDtos);
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