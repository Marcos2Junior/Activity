using System;
using System.Collections.Generic;

namespace API.Models
{
    public class PingTimeActivity
    {
        public int UserId { get; set; }
        public List<DateTime> Pings { get; set; }
        public bool Finished { get; set; }
    }
}
