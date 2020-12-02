using System;

namespace Domain.Entitys
{
    public class TimeActivity
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        public long DateInitial { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public bool Finished { get; set; }
    }
}
