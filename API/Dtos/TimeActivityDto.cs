using API.Models;
using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class TimeActivityDto
    {
        public int Id { get; set; }
        public DateTime DateInitial { get; set; }
        public DateTime? Finish { get; set; }
        public List<DateTime> Pings { get; set; }
    }
}