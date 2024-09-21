using groomroom.Common;
using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var response = new Response();

            response.Data = _context
                .Users
                .Select(x => new UserGetDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Pets = x.Pets.Select(p => new Pets
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Breed = p.Breed,
                        Size = p.Size
                    }).ToList()
                }).ToList();
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(
            [FromRoute] int Id)
        {
            var response = new Response();

            var user = _context.Users.FirstOrDefault(x => x.Id == Id);

            if (user == null)
            {
                response.AddError("Id", "There was a problem finding the user.");
                return NotFound(response);
            }

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

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] UserCreateDto userCreateDto)
        {
            var response = new Response();

            if (string.IsNullOrEmpty(userCreateDto.FirstName))
            {
                response.AddError("firstName", "First name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userCreateDto.LastName))
            {
                response.AddError("lastName", "Last name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userCreateDto.UserName))
            {
                response.AddError("userName", "User name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userCreateDto.Password))
            {
                response.AddError("password", "Password cannot be empty.");
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

            await _userManager.CreateAsync(userToCreate, userCreateDto.Password);
            await _userManager.AddToRoleAsync(userToCreate, "Admin");
            _context.SaveChanges();

            var userGetDto = new UserGetDto
            {
                Id = userToCreate.Id,
                FirstName = userToCreate.FirstName,
                LastName = userToCreate.LastName,
                UserName = userToCreate.UserName
            };

            response.Data = userGetDto;

            return Created("", response);
        }

        [HttpPost("{UserId}/Pets")]
        public IActionResult AddPetToUser(int UserId, [FromBody] PetDto petDto)
        {
            var response = new Response();

            var user = _context.Users.FirstOrDefault(x => x.Id == UserId);
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
            _context.Pets.Add(pet);
            _context.SaveChanges();

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
        public IActionResult Edit(
            [FromRoute] int Id,
            [FromBody] UserUpdateDto userUpdateDto)
        {
            var response = new Response();

            if (userUpdateDto == null)
            {
                response.AddError("Id", "There was a problem editing the user.");
                return NotFound(response);
            }

            var userToEdit = _context.Users.FirstOrDefault(x => x.Id == Id);

            if (userToEdit == null)
            {
                response.AddError("Id", "Could not find user to edit.");
                return NotFound(response);
            }

            if (string.IsNullOrEmpty(userUpdateDto.FirstName))
            {
                response.AddError("firstName", "First name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userUpdateDto.LastName))
            {
                response.AddError("lastName", "Last name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userUpdateDto.UserName))
            {
                response.AddError("userName", "User name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userUpdateDto.Password))
            {
                response.AddError("password", "Password cannot be empty.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            userToEdit.FirstName = userUpdateDto.FirstName;
            userToEdit.LastName = userUpdateDto.LastName;
            userToEdit.UserName = userUpdateDto.UserName;

            _context.SaveChanges();

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
        public IActionResult Delete(int Id)
        {
            var response = new Response();

            var user = _context.Users.FirstOrDefault(x => x.Id == Id);

            if (user == null)
            {
                response.AddError("Id", "There was a problem deleting the user.");
                return NotFound(response);
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(response);
        }
    }
}
