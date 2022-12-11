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
            CreateMap<Bus, BusDto>();
            CreateMap<BusCreateCommand, Bus>();
            CreateMap<BusEditCommand, Bus>();
        }
    }
}
