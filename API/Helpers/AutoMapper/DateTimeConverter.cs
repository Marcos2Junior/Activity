using AutoMapper;
using System;

namespace API.Helpers.AutoMapper
{
    public class DateTimeConverter : ITypeConverter<DateTime, DateTime>
    {
        public DateTime Convert(DateTime source, DateTime destination, ResolutionContext context)
        {
            var inputDate = source;

            if(inputDate.Kind == DateTimeKind.Utc)
            {
                return inputDate.ToLocalTime();
            }

            return inputDate;
        }
    }
}
