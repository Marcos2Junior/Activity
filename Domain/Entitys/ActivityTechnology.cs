namespace Domain.Entitys
{
    public class ActivityTechnology
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        public int TechnologyId { get; set; }
        public Technology Technology { get; set; }
    }
}
