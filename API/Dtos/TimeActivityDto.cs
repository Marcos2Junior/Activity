using System;

namespace API.Dtos
{
    public class TimeActivityDto
    {
        public DateTime DateInitial { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public bool Finished { get; set; }
    }
}