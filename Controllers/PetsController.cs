using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using groomroom.Api.Data;
using groomroom.Api.Features.Pets;
namespace groomroom.Controllers;

[Route("api/pets")]
[ApiController]
public class PetsController : ControllerBase
{
    private readonly DbSet<Pet> pets;
    private readonly DataContext dataContext;

    public PetsController(DataContext dataContext)
    {
        this.dataContext = dataContext;
        pets = dataContext.Set<Pet>();
    }

    [HttpGet]
    public IQueryable<PetDto> GetAllPets()
    {
        return GetPetDtos(pets);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<PetDto> GetPetById(int id)
    {
        var result = GetPetDtos(pets.Where(x => x.Id == id)).FirstOrDefault();
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<PetDto> CreatePet(PetDto dto)
    {
        if (IsInvalid(dto))
        {
            return BadRequest();
        }

        var pet = new Pet
        {
            Name = dto.Name,
            Breed = dto.Breed,
            Size = dto.Size,
            UserId = dto.UserId
        };
        pets.Add(pet);

        dataContext.SaveChanges();

        dto.Id = pet.Id;

        return CreatedAtAction(nameof(GetPetById), new { id = dto.Id }, dto);
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult<PetDto> UpdatePet(int id, PetDto dto)
    {
        if (IsInvalid(dto))
        {
            return BadRequest();
        }

        var pet = pets.FirstOrDefault(x => x.Id == id);
        if (pet == null)
        {
            return NotFound();
        }

        pet.Name = dto.Name;
        pet.Breed = dto.Breed;
        pet.Size = dto.Size;
        pet.UserId = dto.UserId;

        dataContext.SaveChanges();

        dto.Id = pet.Id;

        return Ok(dto);
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeletePet(int id)
    {
        var pet = pets.FirstOrDefault(x => x.Id == id);
        if (pet == null)
        {
            return NotFound();
        }

        pets.Remove(pet);

        dataContext.SaveChanges();

        return Ok();
    }

    private static bool IsInvalid(PetDto dto)
    {
        return string.IsNullOrWhiteSpace(dto.Name) ||
               dto.Name.Length > 100 ||
               string.IsNullOrWhiteSpace(dto.Breed);
    }

    private static IQueryable<PetDto> GetPetDtos(IQueryable<Pet> pets)
    {
        return pets
            .Select(x => new PetDto
            {
                Id = x.Id,
                Name = x.Name,
                Breed = x.Breed,
                Size = x.Size,
                UserId = x.UserId
            });
    }
}
