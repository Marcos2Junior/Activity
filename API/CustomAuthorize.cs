using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Repository.Context;
using System;
using System.Linq;
using System.Security.Claims;

namespace API
{
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        protected readonly ActivityDbContext _context;
        private readonly ILogger<CustomAuthorize> _logger;

        public CustomAuthorize(ActivityDbContext context, ILogger<CustomAuthorize> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated &&
                    !_context.Users.Any(x => x.Email == context.HttpContext.User.FindFirstValue(ClaimTypes.Email)))
                {
                    context.Result = new UnauthorizedObjectResult("sorry, you are not authenticated");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                context.Result = new ObjectResult(new { StatusCode = 500, Value = "sorry, an error occurred while processing the data" });
            }
        }
    }
}
