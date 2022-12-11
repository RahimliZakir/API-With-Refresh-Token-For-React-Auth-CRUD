using Application.WebAPI.AppCode.Application.Modules.BusModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.Entities;
using AutoMapper;

namespace Application.WebAPI.AppCode.Mappers.Profiles
{
    public class BusProfile : Profile
    {
        public BusProfile()
        {
            CreateMap<Bus, BusDto>()
                     .ForMember(dest => dest.ImagePath, src => src.MapFrom(map => $"https://localhost:7074/uploads/buses/{map.ImagePath}"));
            CreateMap<BusCreateCommand, Bus>();
            CreateMap<BusEditCommand, Bus>();
        }
    }
}
