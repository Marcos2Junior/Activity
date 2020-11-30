using API.Dtos;
using AutoMapper;
using Domain.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/activity")]
    [ApiController]
    [Authorize]
    public class ActivityController : MainController
    {
        private readonly IActivityRepository ActivityRepository;
        private readonly IMapper Mapper;
        public ActivityController(ILogger<ControllerBase> logger, IMapper mapper, IActivityRepository activityRepository) : base(logger, activityRepository)
        {
            Mapper = mapper;
            ActivityRepository = activityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await GetUserAuthAsync();

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("new-activity")]
        public async Task<IActionResult> NewActivity(InsertActivityDto insertActivityDto)
        {
            try
            {
                var user = await GetUserAuthAsync();

                if(user == null) { return BadRequest("user is not authenticated"); }

                var activity = Mapper.Map<Activity>(insertActivityDto);
                activity.Date = DateTime.UtcNow;
                activity.UserId = user.Id;

                if(await ActivityRepository.AddAsync(activity))
                {
                    return Created(nameof(Get), activity);
                }

                return BadRequest();


            }
            catch (Exception ex)
            {
                return ErrorException(ex, nameof(NewActivity));
            }
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartTimeActivity(int idActivity)
        {
            return Ok();
        }

        [HttpPost("ping")]
        public async Task<IActionResult> PingTimeActivity()
        {
            return Ok();
        }
    }
}
