using System.Linq;
using System.Xml.Linq;
using groomroom.Common;
using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers;

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

                Recipes = x.Recipes.Select(x => new UserRecipesGetDto
                {
                    Id = x.Recipes.Id,
                    Name = x.Recipes.Name,
                    BodyText = x.Recipes.BodyText,
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
            Recipes = user.Recipes.Select(x => new UserRecipesGetDto
            {
                Id = x.Recipes.Id,
                Name = x.Recipes.Name,
                BodyText = x.Recipes.BodyText,
            }).ToList()
        };

        response.Data = userGetDto;

        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create(
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
        };

        _userManager.CreateAsync(userToCreate, userCreateDto.Password);
        _userManager.AddToRoleAsync(userToCreate, "Admin");
        _context.SaveChanges();

        var RecipesToReturn = new UserGetDto
        {
            Id = userToCreate.Id,
            FirstName = userToCreate.FirstName,
            LastName = userToCreate.LastName,
            UserName = userToCreate.UserName
        };

        response.Data = RecipesToReturn;

        return Created("", response);
    }

    //here
    [HttpPost("{UserId}/Recipes/{RecipeId}")]
    public IActionResult AddRecipeToUser(int UserId, int RecipeId)
    {
        var response = new Response();

        var User = _context.Set<User>()
            .FirstOrDefault(x => x.Id == UserId);

        var Recipes = _context.Set<Recipes>()
            .FirstOrDefault(x => x.Id == RecipeId);

        var UserRecipes = new UserRecipes
        {
            User = User,
            Recipes = Recipes
        };
        _context.Set<UserRecipes>().Add(UserRecipes);
        _context.SaveChanges();

        response.Data = new UserGetDto
        {
            Id = User.Id,
            FirstName = User.FirstName,
            LastName = User.LastName,
            UserName = User.UserName,

            Recipes = User.Recipes.Select(x => new UserRecipesGetDto
            {
                Id = x.Recipes.Id,
                Name = x.Recipes.Name,
                BodyText = x.Recipes.BodyText,


            }).ToList()
        };

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