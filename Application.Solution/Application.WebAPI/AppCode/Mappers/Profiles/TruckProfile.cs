using Application.WebAPI.AppCode.Application.Modules.TruckModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.Entities;
using AutoMapper;

namespace Application.WebAPI.AppCode.Mappers.Profiles
{
    public class TruckProfile : Profile
    {
        public TruckProfile()
        {
            CreateMap<Truck, TruckDto>();
            CreateMap<TruckCreateCommand, Truck>();
            CreateMap<TruckEditCommand, Truck>();
        }
    }
}
