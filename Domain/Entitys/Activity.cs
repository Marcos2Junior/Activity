using Domain.Enum;
using System;
using System.Collections.Generic;

namespace Domain.Entitys
{
    public class Activity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public TypeActivity TypeActivity { get; set; }
        public string DetailsFinish { get; set; }
        public long? ExpectecStartDate { get; set; }
        public long? FinishMaximum { get; set; }
        public long? StartDate { get; set; }
        public long? FinishDate { get; set; }
        public long Date { get; set; }
        public List<TimeActivity> TimeActivities { get; set; }
        public List<ActivityTechnology> ActivityTechnologies { get; set; }
    }
}
