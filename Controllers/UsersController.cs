using groomroom.Common;
using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(DataContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response();

            var users = await _context.Users
                .Include(u => u.Pets)
                .Include(u => u.Appointments)
                .ToListAsync();
            
            var userDtos = new List<UserGetDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);               var userGetDto = new UserGetDto
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

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var response = new Response();
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                response.AddError("userId", "User is not logged in.");
                return Unauthorized(response);
            }

            var user = await _context.Users
                .Include(u => u.Pets)
                .Include(u => u.Appointments)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Id == int.Parse(userId));

            if (user == null)
            {
                response.AddError("Id", "There was a problem finding the user.");
                return NotFound(response);
            }

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

            response.Data = userGetDto;

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] UserCreateDto userCreateDto)
        {
            var response = new Response();

            if (string.IsNullOrEmpty(userCreateDto.FirstName))
            {
                response.AddError("firstName", "First name cannot be empty.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var userToCreate = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                UserName = userCreateDto.UserName,
                Email = userCreateDto.Email
            };

            var result = await _userManager.CreateAsync(userToCreate, userCreateDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddError("createUser", error.Description);
                }
                return BadRequest(response);
            }

            await _userManager.AddToRoleAsync(userToCreate, "User");

            var userGetDto = new UserGetDto
            {
                Id = userToCreate.Id,
                FirstName = userToCreate.FirstName,
                LastName = userToCreate.LastName,
                UserName = userToCreate.UserName,
                Email = userToCreate.Email
            };

            response.Data = userGetDto;

            return Created("", response);
        }

        [HttpPost("add-pet")]
        public async Task<IActionResult> AddPetToUser([FromBody] PetDto petDto)
        {
            var response = new Response();
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                response.AddError("userId", "User is not logged in.");
                return Unauthorized(response);
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
            if (user == null)
            {
                response.AddError("UserId", "User not found.");
                return NotFound(response);
            }

            var pet = new Pets
            {
                Name = petDto.Name,
                Breed = petDto.Breed,
                Size = petDto.Size,
                UserId = int.Parse(userId),
            };

            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();

            var userGetDto = new UserGetDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Pets = user.Pets.Select(p => new Pets
                {
                    Id = p.Id,
                    Name = p.Name,
                    Breed = p.Breed,
                    Size = p.Size
                }).ToList()
            };

            response.Data = userGetDto;
            return Ok(response);
        }

        [HttpPost("book-appointment")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentCreateDto appointmentCreateDto)
        {
            var response = new Response();
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                response.AddError("userId", "User is not logged in.");
                return Unauthorized(response);
            }

            if (appointmentCreateDto == null || string.IsNullOrEmpty(appointmentCreateDto.Date) || 
                appointmentCreateDto.ServiceId == null || !appointmentCreateDto.ServiceId.Any() || 
                appointmentCreateDto.ServiceId.Any(s => s <= 0))
            {
                response.AddError("appointment", "Invalid appointment details.");
                return BadRequest(response);
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
            if (user == null)
            {
                response.AddError("userId", "User not found.");
                return NotFound(response);
            }

            DateTime appointmentDate;
            if (!DateTime.TryParseExact(appointmentCreateDto.Date, "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out appointmentDate))
            {
                response.AddError("date", "Invalid date format. Please use MM-dd-yyyy.");
                return BadRequest(response);
            }

            var servicePrices = await _context.Services
                .Where(s => appointmentCreateDto.ServiceId.Contains(s.Id))
                .Select(s => s.Price)
                .ToListAsync();

            decimal total = servicePrices.Sum();

            var appointment = new Appointment
            {
                Date = appointmentDate,
                UserId = int.Parse(userId),
                ServiceId = appointmentCreateDto.ServiceId,
                Total = total,
            };

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            var appointmentResponse = new AppointmentDto
            {
                ServiceId = appointment.ServiceId,
                Date = appointment.Date.ToString("MM-dd-yyyy")
            };

            response.Data = appointmentResponse;
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UserUpdateDto userUpdateDto)
        {
            var response = new Response();
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                response.AddError("userId", "User is not logged in.");
                return Unauthorized(response);
            }

            var userToEdit = await _context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
            if (userToEdit == null)
            {
                response.AddError("Id", "Could not find user to edit.");
                return NotFound(response);
            }

            if (!string.IsNullOrEmpty(userUpdateDto.FirstName))
            {
                userToEdit.FirstName = userUpdateDto.FirstName;
            }

            if (!string.IsNullOrEmpty(userUpdateDto.LastName))
            {
                userToEdit.LastName = userUpdateDto.LastName;
            }

            if (!string.IsNullOrEmpty(userUpdateDto.UserName))
            {
                userToEdit.UserName = userUpdateDto.UserName;
            }

            await _context.SaveChangesAsync();

            var userGetDto = new UserGetDto
            {
                Id = userToEdit.Id,
                FirstName = userToEdit.FirstName,
                LastName = userToEdit.LastName,
                UserName = userToEdit.UserName,
            };

            response.Data = userGetDto;
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var response = new Response();
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                response.AddError("userId", "User is not logged in.");
                return Unauthorized(response);
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
            if (user == null)
            {
                response.AddError("Id", "There was a problem deleting the user.");
                return NotFound(response);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
    }
}
