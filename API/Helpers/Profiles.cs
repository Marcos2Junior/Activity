using API.Dtos;
using AutoMapper;
using Domain.Entitys;

namespace API.Helpers
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<User, UserViewDto>().ReverseMap();
            CreateMap<User, UserInsertDto>().ReverseMap();
            CreateMap<Activity, InsertActivityDto>().ReverseMap();
        }
    }
}
