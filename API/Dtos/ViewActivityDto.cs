using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ViewActivityDto
    {
        [Required(ErrorMessage = "Activity id is required")]
        public int Id { get; set; }
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
        public List<TimeActivityDto> TimeActivitiesDto { get; set; }
        public List<ActivityTechnologyDto> ActivityTechnologiesDto { get; set; }
    }
}
