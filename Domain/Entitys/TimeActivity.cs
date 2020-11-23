using System;

namespace Domain.Entitys
{
    public class TimeActivity
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        public DateTime DateInitial { get; set; }
        public DateTime? Finish { get; set; }
    }
}
