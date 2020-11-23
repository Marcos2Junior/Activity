using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositorys
{
    public class ActivityRepository : MainRepository, IActivityRepository
    {
        public ActivityRepository(ActivityDbContext context) : base(context)
        {
        }

        public async Task<Activity> GetActivityByIdAsync(int userId) =>
                await _context.Activitys
                .Include(x => x.TimeActivities)
                .Include(x => x.ActivityTechnologies).ThenInclude(x => x.Technology)
                .FirstOrDefaultAsync(x => x.UserId == userId);


        public async Task<List<Activity>> GetAllActivityAsync(int userId, DateTime dateInit, DateTime dateFinish, bool includeTime = false, bool includeTechnology = false)
        {
            List<Activity> activities = new List<Activity>();

            var query = from activity in _context.Activitys where activity.UserId == userId && activity.Date >= dateInit && activity.Date <= dateFinish select activity;

            if (includeTime && includeTechnology)
            {
                activities = await query
                     .Include(x => x.TimeActivities)
                     .Include(x => x.ActivityTechnologies).ThenInclude(x => x.Technology)
                     .ToListAsync();
            }
            else if (includeTime)
            {
                activities = await query
                .Include(x => x.TimeActivities)
                .ToListAsync();
            }
            else if (includeTechnology)
            {
                activities = await query
               .Include(x => x.ActivityTechnologies).ThenInclude(x => x.Technology)
               .ToListAsync();
            }
            else
            {
                activities = await query.ToListAsync();
            }

            return activities
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Priority)
                .ThenBy(x => x.TypeActivity)
                .ThenBy(x => x.Name)
                .ToList();
        }
    }
}
