﻿using Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IActivityRepository : IMainRepository
    {
        Task<Activity> GetActivityByIdAsync(int userId);
        Task<List<Activity>> GetAllActivityAsync(int userId, DateTime dateInit, DateTime dateFinish, bool includeTime = false, bool includeTechnology = false);
    }
}
