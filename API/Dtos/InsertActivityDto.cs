using Domain.Enum;
using System;

namespace API.Dtos
{
    public class InsertActivityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public TypeActivity TypeActivity { get; set; }
        public DateTime? ExpectecStartDate { get; set; }
        public DateTime? FinishMaximum { get; set; }
    }
}
