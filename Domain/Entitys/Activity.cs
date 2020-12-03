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
        public DateTime? ExpectecStartDate { get; set; }
        public DateTime? FinishMaximum { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime Date { get; set; }
        public List<TimeActivity> TimeActivities { get; set; }
        public List<ActivityTechnology> ActivityTechnologies { get; set; }
    }
}
