using AutoMapper;
using System;

namespace API.Helpers.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static Action<IMapperConfigurationExpression> Configure()
        {
            return new Action<IMapperConfigurationExpression>(x =>
            {
                x.CreateMap<DateTime, DateTime>().ConvertUsing<DateTimeConverter>();
            });
        }
    }
}
