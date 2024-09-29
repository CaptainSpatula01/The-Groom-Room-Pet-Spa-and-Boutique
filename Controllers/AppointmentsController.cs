using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace groomroom.Api.Controllers;

[Route("api/appointments")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly DataContext dataContext;
    private readonly DbSet<Appointment> appointments;

    public AppointmentsController(DataContext dataContext)
    {
        this.dataContext = dataContext;
        appointments = dataContext.Set<Appointment>();
    }

    [HttpGet]
    [Route("user/{userId}")]
    public ActionResult<IEnumerable<PetDto>> GetPetByUserId(int userId)
    {
        var userAppointments = appointments
            .Where(p => p.UserId == userId)
            .Select(x => new AppointmentDto
            {
                Date = x.Date.ToString("MM-dd-yyyy"),
                ServiceId = x.ServiceId,
            })
            .ToList();
            if (!userAppointments.Any())
        {
            return NotFound("No appointments found for user.");
        }

        return Ok(userAppointments);
    }

    [HttpGet]
    public IQueryable<AppointmentDto> GetAllAppointments()
    {
        return GetAppointmentDtos(appointments);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<AppointmentDto> GetAppointmentById(int id)
    {
        var result = GetAppointmentDtos(appointments.Where(x => x.Id == id)).FirstOrDefault();
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointmentAsync(AppointmentDto dto)
    {
        if (IsInvalid(dto))
        {
            return BadRequest();
        }

        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized("User is not authenticated.");
        }

        var appointment = new Appointment
        {
            Date = DateTime.ParseExact(dto.Date, "MM-dd-yyyy", null),
            UserId = int.Parse(currentUserId),
            ServiceId = dto.ServiceId,
        };
        await appointments.AddAsync(appointment);
        await dataContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, dto);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<AppointmentDto>> UpdateAppointmentAsync(int id, AppointmentDto dto)
    {
        if (IsInvalid(dto))
        {
            return BadRequest();
        }

        var appointment = appointments.FirstOrDefault(x => x.Id == id);
        if (appointment == null)
        {
            return NotFound();
        }

        appointment.Date = DateTime.ParseExact(dto.Date, "MM-dd-yyyy", null);
        appointment.ServiceId = dto.ServiceId;

        await dataContext.SaveChangesAsync();

        return Ok(dto);
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeleteAppointment(int id)
    {
        var appointment = appointments.FirstOrDefault(x => x.Id == id);
        if (appointment == null)
        {
            return NotFound();
        }

        appointments.Remove(appointment);

        dataContext.SaveChanges();

        return Ok();
    }

    private static bool IsInvalid(AppointmentDto dto)
    {
        return dto.ServiceId == null || dto.ServiceId.Count == 0 ||
               !DateTime.TryParseExact(dto.Date, "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out _);
    }

    private static IQueryable<AppointmentDto> GetAppointmentDtos(IQueryable<Appointment> appointments)
    {
        return appointments
            .Select(x => new AppointmentDto
            {
                Date = x.Date.ToString("MM-dd-yyyy"),
                ServiceId = x.ServiceId,
            });
    }
}
