using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/activity")]
    [ApiController]
    public class ActivityController : MainController
    {
        private readonly IActivityRepository ActivityRepository;
        private readonly IMapper Mapper;
        public ActivityController(ILogger<ControllerBase> logger, IMapper mapper, IActivityRepository activityRepository) : base(logger, activityRepository)
        {
            Mapper = mapper;
            ActivityRepository = activityRepository;
        }

        [HttpGet("get-activity")]
        public async Task<IActionResult> Get(int activityId)
        {
            var activityResult = await SelectActivityAsync(activityId);

            if (activityResult.StatusCode != 200) { return activityResult; }

            var activity = Mapper.Map<ViewActivityDto>(activityResult.Value);

            return Ok(activity);
        }

        private async Task<ObjectResult> SelectActivityAsync(int idActivity)
        {
            try
            {
                var user = await GetUserAuthAsync();

                var activity = await ActivityRepository.GetActivityByIdAsync(idActivity);

                if (activity != null)
                {
                    if (activity.UserId == user.Id)
                    {
                        return StatusCode(200, activity);
                    }
                    else
                    {
                        return BadRequest("this activity is not referenced to user authenticated");
                    }
                }
                else
                {
                    return NotFound("id activity is not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("new-activity")]
        public async Task<IActionResult> NewActivity(InsertActivityDto insertActivityDto)
        {
            try
            {
                var user = await GetUserAuthAsync();

                var activity = Mapper.Map<Activity>(insertActivityDto);
                activity.Date = DateTime.UtcNow;
                activity.UserId = user.Id;

                if (await ActivityRepository.AddAsync(activity))
                {
                    var activityReturn = Mapper.Map<ViewActivityDto>(activity);

                    return Created(nameof(Get), activityReturn);
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
            try
            {
                var activityObject = await SelectActivityAsync(idActivity);

                if (activityObject.StatusCode != 200) { return activityObject; }

                var activity = (Activity)activityObject.Value;

                if (activity.TimeActivities.Any(x => !x.Finished) || !activity.TimeActivities.Any())
                {
                    await ActivityRepository.AddAsync(new TimeActivity
                    {
                        ActivityId = activity.Id,
                        DateInitial = DateTime.UtcNow,
                        Finished = false
                    });

                    return Ok();
                }
                else
                {
                    //Informa ao usuario de que ele não pode iniciar duas atividades com timer ao mesmo tempo
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return ErrorException(ex, nameof(StartTimeActivity));
            }
        }

        [HttpPost("ping")]
        public async Task<IActionResult> PingTimeActivity(TimeSpan timeSpan)
        {
            try
            {
                var user = await GetUserAuthAsync();

                await ActivityRepository.PingTimeActivityAsync(user.Id, timeSpan);

                return Ok();
            }
            catch (Exception ex)
            {
                return ErrorException(ex, nameof(PingTimeActivity));
            }
        }
    }
}
