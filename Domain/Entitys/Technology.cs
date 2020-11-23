using System;
using System.Collections.Generic;

namespace Domain.Entitys
{
    public class Technology
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public List<ActivityTechnology> ActivityTechnologies { get; set; }
    }
}