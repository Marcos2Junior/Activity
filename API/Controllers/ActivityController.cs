using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Entitys;
using Microsoft.AspNetCore.Authorization;
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

            if (user == null)
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

                if (user == null) { return BadRequest("user is not authenticated"); }

                var activity = Mapper.Map<Activity>(insertActivityDto);
                activity.Date = DateTime.UtcNow;
                activity.UserId = user.Id;

                if (await ActivityRepository.AddAsync(activity))
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
            try
            {
                var user = await GetUserAuthAsync();

                var activity = await ActivityRepository.GetActivityByIdAsync(idActivity);

                if (activity != null)
                {
                    if (activity.UserId == user.Id)
                    {
                        if (activity.TimeActivities.Any(x => !x.Finish.HasValue) || !activity.TimeActivities.Any())
                        {
                            await ActivityRepository.AddAsync(new TimeActivity
                            {
                                ActivityId = activity.Id,
                                DateInitial = DateTime.UtcNow
                            });

                            ActivityStartedService.Add(user.Id);

                            return Ok();
                        }
                        else
                        {
                            //Informa ao usuario de que ele não pode iniciar duas atividades com timer ao mesmo tempo
                            return NoContent();
                        }
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
                return ErrorException(ex, nameof(StartTimeActivity));
            }
        }

        [HttpPost("ping")]
        public async Task<IActionResult> PingTimeActivity()
        {
            try
            {
                var user = await GetUserAuthAsync();
                ActivityStartedService.PingActivity(user.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return ErrorException(ex, nameof(PingTimeActivity));
            }
        }
    }
}
