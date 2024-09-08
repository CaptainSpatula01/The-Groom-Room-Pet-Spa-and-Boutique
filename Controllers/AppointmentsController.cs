using System;
using System.Linq;
using groomroom.Common;
using groomroom.Data;
using groomroom.Entities;
using groomroom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace groomroom.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAuthenticationService _authenticationService;
        public AppointmentsController(DataContext dataContext, )
    }
}
