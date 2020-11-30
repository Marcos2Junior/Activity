using System;

namespace API.Dtos
{
    public class TimeActivityDto
    {
        public int Id { get; set; }
        public DateTime DateInitial { get; set; }
        public DateTime? Finish { get; set; }
    }
}