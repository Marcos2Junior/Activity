using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace API.Controllers
{
    public class MainController : ControllerBase
    {
        protected readonly ILogger<ControllerBase> _logger;

        public MainController(ILogger<ControllerBase> logger)
        {
            _logger = logger;
        }
        protected IActionResult ErrorException(Exception exception, string method, int? id = null)
        {
            string _id = id != null ? $", id: {id}" : string.Empty;

            _logger.LogError(exception, $"exception lancada metodo {method}{_id}");

            return StatusCode(500, "Ocorreu um erro interno com o tratamento dos dados.");
        }
    }
}
