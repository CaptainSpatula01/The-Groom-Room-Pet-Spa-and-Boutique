using groomroom.Data;
using groomroom.Entities;
using IdentityModel;
using System.Security.Claims;

namespace LearningStarter.Services
{
    public interface IAuthenticationService
    {
        User GetLoggedInUser();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(
            DataContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetLoggedInUser()
        {
            if (!IsUserLoggedIn())
                return null;

            var idClaim = RequestingUser.FindFirstValue(JwtClaimTypes.Subject);
            if (int.TryParse(idClaim, out var id))
            {
                return _context.Users.SingleOrDefault(x => x.Id == id);
            }

            return null;
        }

        private ClaimsPrincipal RequestingUser
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var identity = httpContext?.User.Identity;

                if (identity == null)
                {
                    return null;
                }

                return !identity.IsAuthenticated
                    ? null
                    : httpContext.User;
            }
        }

        private bool IsUserLoggedIn()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
