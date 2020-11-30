using System;
namespace API.Models
{
    public class PingTimeActivity
    {
        public int UserId { get; set; }
        public DateTime LastPing { get; set; }
    }
}
