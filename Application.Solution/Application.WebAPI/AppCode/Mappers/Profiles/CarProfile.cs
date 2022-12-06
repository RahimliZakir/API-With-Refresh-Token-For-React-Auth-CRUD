using Application.WebAPI.AppCode.Application.Modules.CarModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.Entities;
using AutoMapper;

namespace Application.WebAPI.AppCode.Mappers.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarDto>();
            CreateMap<CarCreateCommand, Car>();
            CreateMap<CarEditCommand, Car>();
        }
    }
}
