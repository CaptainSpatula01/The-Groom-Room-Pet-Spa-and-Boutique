using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using groomroom.Api.Data;
using groomroom.Api.Features.Services;

namespace groomroom.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly DbSet<Service> services;
        private readonly DataContext dataContext;

        public ServicesController(DataContext dataContext)
        {
            this.dataContext = dataContext;
            services = dataContext.Set<Service>();
        }

        [HttpGet]
        public IQueryable<ServiceDto> GetAllServices()
        {
            return GetServiceDtos(services);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<ServiceDto> GetServiceById(int id)
        {
            var result = GetServiceDtos(services.Where(x => x.Id == id)).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<ServiceDto> CreateService(ServiceDto dto)
        {
            if (IsInvalid(dto))
            {
                return BadRequest();
            }

            var service = new Service
            {
                Price = dto.Price,
                Description = dto.Description
            };
            services.Add(service);

            dataContext.SaveChanges();

            dto.Id = service.Id;

            return CreatedAtAction(nameof(GetServiceById), new { id = dto.Id }, dto);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<ServiceDto> UpdateService(int id, ServiceDto dto)
        {
            if (IsInvalid(dto))
            {
                return BadRequest();
            }

            var service = services.FirstOrDefault(x => x.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            service.Price = dto.Price;
            service.Description = dto.Description;

            dataContext.SaveChanges();

            dto.Id = service.Id;

            return Ok(dto);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteService(int id)
        {
            var service = services.FirstOrDefault(x => x.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            services.Remove(service);

            dataContext.SaveChanges();

            return Ok();
        }

        private static bool IsInvalid(ServiceDto dto)
        {
            return dto.Price <= 0 || string.IsNullOrWhiteSpace(dto.Description) || dto.Description.Length > 500;
        }

        private static IQueryable<ServiceDto> GetServiceDtos(IQueryable<Service> services)
        {
            return services
                .Select(x => new ServiceDto
                {
                    Id = x.Id,
                    Price = x.Price,
                    Description = x.Description
                });
        }
    }
