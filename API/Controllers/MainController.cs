using Domain.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class MainController : ControllerBase
    {
        protected readonly ILogger<ControllerBase> _logger;
        protected readonly IMainRepository MainRepository;

        public MainController(ILogger<ControllerBase> logger, IMainRepository mainRepository)
        {
            _logger = logger;
            MainRepository = mainRepository;
        }
        protected IActionResult ErrorException(Exception exception, string method, int? id = null)
        {
            string _id = id != null ? $", id: {id}" : string.Empty;

            _logger.LogError(exception, $"exception lancada metodo {method}{_id}");

            return StatusCode(500, "Ocorreu um erro interno com o tratamento dos dados.");
        }

        protected async Task<User> GetUserAuthAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            return await MainRepository.GetWhereFirstEntityAsync<User>(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
        }
    }
}
