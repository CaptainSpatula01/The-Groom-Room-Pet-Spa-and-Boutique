using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace groomroom.Controllers
{

    [Authorize]
    [Route("api/pets")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly DbSet<Pets> pets;
        private readonly DataContext dataContext;

        public PetsController(DataContext dataContext)
        {
            this.dataContext = dataContext;
            pets = dataContext.Set<Pets>();
        }

        [HttpGet]
        [Route("user/{userId}")]
        public ActionResult<IEnumerable<PetDto>> GetPetByUserId(int userId)
        {
            var userPets = pets
                .Where(p => p.UserId == userId)
                .Select(x => new PetDto
                {
                    Name = x.Name,
                    Breed = x.Breed,
                    Size = x.Size,
                })
                .ToList();

            if (!userPets.Any())
            {
                return NotFound("No pets found for user.");
            }

            return Ok(userPets);
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

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var pet = new Pets
            {
                Name = dto.Name,
                Breed = dto.Breed,
                Size = dto.Size,
                UserId = userId,
            };
            pets.Add(pet);

            dataContext.SaveChanges();

            var responseDto = new PetDto
            {
                Name = pet.Name,
                Breed = pet.Breed,
                Size = pet.Size
            };

            return CreatedAtAction(nameof(GetPetById), new { id = pet.Id }, responseDto);
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

            dataContext.SaveChanges();

            var responseDto = new PetDto
            {
                Name = pet.Name,
                Breed = pet.Breed,
                Size = pet.Size
            };

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

        private static IQueryable<PetDto> GetPetDtos(IQueryable<Pets> pets)
        {
            return pets
                .Select(x => new PetDto
                {
                    Name = x.Name,
                    Breed = x.Breed,
                    Size = x.Size,
                });
        }
    }
}
