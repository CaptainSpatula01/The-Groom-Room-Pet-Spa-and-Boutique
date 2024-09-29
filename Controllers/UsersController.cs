using groomroom.Common;
using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Controllers
{
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
                    UserRoles = roles.ToList() 
                };

                userDtos.Add(userGetDto);
            }

            response.Data = userDtos;

            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var response = new Response();

            var user = await _context.Users
                .Include(u => u.Pets)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Id == Id);

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
                UserRoles = roles.ToList() 
            };

            response.Data = userGetDto;

            return Ok(response);
        }

        [HttpPost]
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

        [HttpPost("{UserId}/Pets")]
        public async Task<IActionResult> AddPetToUser(int UserId, [FromBody] PetDto petDto)
        {
            var response = new Response();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
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
                User = user
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

        [HttpPut("{Id}")]
        public async Task<IActionResult> Edit([FromRoute] int Id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var response = new Response();

            if (userUpdateDto == null)
            {
                response.AddError("Id", "There was a problem editing the user.");
                return NotFound(response);
            }

            var userToEdit = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (userToEdit == null)
            {
                response.AddError("Id", "Could not find user to edit.");
                return NotFound(response);
            }

            if (string.IsNullOrEmpty(userUpdateDto.FirstName))
            {
                response.AddError("firstName", "First name cannot be empty.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            userToEdit.FirstName = userUpdateDto.FirstName;
            userToEdit.LastName = userUpdateDto.LastName;
            userToEdit.UserName = userUpdateDto.UserName;

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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = new Response();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
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
