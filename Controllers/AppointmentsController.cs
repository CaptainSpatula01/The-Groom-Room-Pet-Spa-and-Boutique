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
                Id = x.Id,
                Date = x.Date.ToString("MM-dd-yyyy"),
                UserId = x.UserId,
                ServiceId = x.ServiceId,
                Total = x.Total
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
    public ActionResult<AppointmentDto> CreateAppointment(AppointmentDto dto)
    {
        if (IsInvalid(dto))
        {
            return BadRequest();
        }

        var appointment = new Appointment
        {
            Date = DateTime.ParseExact(dto.Date, "MM-dd-yyyy", null),
            UserId = dto.UserId,
            ServiceId = dto.ServiceId,
            Total = dto.Total
        };
        appointments.Add(appointment);

        dataContext.SaveChanges();

        dto.Id = appointment.Id;

        return CreatedAtAction(nameof(GetAppointmentById), new { id = dto.Id }, dto);
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult<AppointmentDto> UpdateAppointment(int id, AppointmentDto dto)
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
        appointment.UserId = dto.UserId;
        appointment.ServiceId = dto.ServiceId;
        appointment.Total = dto.Total;

        dataContext.SaveChanges();

        dto.Id = appointment.Id;

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
        return dto.UserId <= 0 ||
               dto.ServiceId == null || dto.ServiceId.Count == 0 ||
               dto.Total <= 0 ||
               !DateTime.TryParseExact(dto.Date, "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out _);
    }

    private static IQueryable<AppointmentDto> GetAppointmentDtos(IQueryable<Appointment> appointments)
    {
        return appointments
            .Select(x => new AppointmentDto
            {
                Id = x.Id,
                Date = x.Date.ToString("MM-dd-yyyy"),
                UserId = x.UserId,
                ServiceId = x.ServiceId,
                Total = x.Total
            });
    }
}
