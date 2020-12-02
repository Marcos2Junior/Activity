using API.Dtos;
using AutoMapper;
using Domain.Entitys;
using System.Linq;

namespace API.Helpers.AutoMapper
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<User, UserViewDto>().ReverseMap();
            CreateMap<User, UserInsertDto>().ReverseMap();
            CreateMap<Activity, InsertActivityDto>().ReverseMap();
            CreateMap<TimeActivity, TimeActivityDto>().ReverseMap();
            CreateMap<Activity, ViewActivityDto>()
                .ForMember(
                    dest => dest.TechnologiesDto, 
                    opt => opt.MapFrom(src => src.ActivityTechnologies.Select(x => x.Technology)))
                .ForMember(
                    dest => dest.TimeActivitiesDto,
                    opt => opt.MapFrom(src => src.TimeActivities))
                .ReverseMap();
        }
    }
}
