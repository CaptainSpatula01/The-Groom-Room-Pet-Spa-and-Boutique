using System;
using System.Linq;
using groomroom.Common;
using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Mvc;

namespace groomroom.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context) {
            _context = context;
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
                    Username = x.Username,
                    Email = x.Email,
                    DateCreated = x.DateCreated,
                    IsDeleted = x.IsDeleted,
                })
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(
            [FromRoute] int id)
        {
            var response = new Response();

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                response.AddError("id", "There was a problem finding the user.");
                return NotFound(response);
            }

            var userGetDto = new UserGetDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                DateCreated = user.DateCreated,
                IsDeleted = user.IsDeleted,
            };

            response.Data = userGetDto;

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] UserCreateDto userCreateDto)
        {
            var response = new Response();

            var userNameAlreadyExists = _context
                    .Users.Any(x => x.Username == userCreateDto.Username);

            var emailAlreadyExists = _context
                    .Users.Any(x => x.Email == userCreateDto.Email);

            if (string.IsNullOrEmpty(userCreateDto.FirstName.Trim()))
            {
                response.AddError("firstName", "First name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userCreateDto.LastName.Trim()))
            {
                response.AddError("lastName", "Last name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userCreateDto.Username.Trim()))
            {
                response.AddError("userName", "User name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userCreateDto.Password.Trim()))
            {
                response.AddError("password", "Password cannot be empty.");
            }
            if (string.IsNullOrEmpty(userCreateDto.Email.Trim()))
            {
                response.AddError("email", "Email cannot be empty.");
            }
            if (userNameAlreadyExists)
            {
                response.AddError("username", "Username already in use.");
            }
            if (emailAlreadyExists)
            {
                response.AddError("email", "Email already in use");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var userToCreate = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Username = userCreateDto.Username,
                Password = userCreateDto.Password,
                Email = userCreateDto.Email,
                DateCreated = DateTimeOffset.Now
            };

            _context.Users.Add(userToCreate);
            _context.SaveChanges();

            var userGetDto = new UserGetDto
            {
                Id = userToCreate.Id,
                FirstName = userToCreate.FirstName,
                LastName = userToCreate.LastName,
                Username = userToCreate.Username,
                Email = userToCreate.Email,
                DateCreated = DateTimeOffset.Now,

            };

            response.Data = userGetDto;

            return Created("", response);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(
            [FromRoute] int id, 
            [FromBody] UserUpdateDto userUpdateDto)
        {
            var response = new Response();
            
            var userNameAlreadyExists = _context
                    .Users.Any(x => x.Username == userUpdateDto.Username && !(x.Id == id));

            var emailAlreadyExists = _context
                    .Users.Any(x => x.Email == userUpdateDto.Email && !(x.Id == id));

            if (userUpdateDto == null)
            {
                response.AddError("id", "There was a problem editing the user.");
                return NotFound(response);
            }
            
            var userToEdit = _context.Users.FirstOrDefault(x => x.Id == id);

            if (userToEdit == null)
            {
                response.AddError("id", "Could not find user to edit.");
                return NotFound(response);
            }

            if (string.IsNullOrEmpty(userUpdateDto.FirstName.Trim()))
            {
                response.AddError("firstName", "First name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userUpdateDto.LastName.Trim()))
            {
                response.AddError("lastName", "Last name cannot be empty.");
            }

            if (string.IsNullOrEmpty(userUpdateDto.Username.Trim()))
            {
                response.AddError("userName", "User name cannot be empty.");
            }

            if(string.IsNullOrEmpty(userUpdateDto.Email.Trim()))
            {
                response.AddError("email", "Email cannot be empty.");
            }
            if (userNameAlreadyExists)
            {
                response.AddError("username", "Username already in use.");
            }
            if (emailAlreadyExists)
            {
                response.AddError("email", "Email already in use");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            userToEdit.FirstName = userUpdateDto.FirstName;
            userToEdit.LastName = userUpdateDto.LastName;
            userToEdit.Username = userUpdateDto.Username;
            userToEdit.Email = userUpdateDto.Email;

            _context.SaveChanges();

            var userGetDto = new UserGetDto
            {
                Id = userToEdit.Id,
                FirstName = userToEdit.FirstName,
                LastName = userToEdit.LastName,
                Username = userToEdit.Username,
                Email = userToEdit.Email,
                DateCreated = userToEdit.DateCreated,
                IsDeleted = userToEdit.IsDeleted,
            };

            response.Data = userGetDto;

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                response.AddError("id", "There was a problem deleting the user.");
                return NotFound(response);
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            response.Data = true;
            return Ok(response);
        }
    }
}
