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
            CreateMap<Truck, TruckDto>()
                     .ForMember(dest => dest.ImagePath, src => src.MapFrom(map => $"https://localhost:7074/uploads/trucks/{map.ImagePath}"));
            CreateMap<TruckCreateCommand, Truck>();
            CreateMap<TruckEditCommand, Truck>();
        }
    }
}
