using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.Entities;
using AutoMapper;

namespace Application.WebAPI.AppCode.Mappers.Profiles
{
    public class CarImageProfile : Profile
    {
        public CarImageProfile()
        {
            CreateMap<CarImage, CarImageDto>()
                     .ForMember(dest => dest.ImagePath, src => src.MapFrom(map => $"https://localhost:7074/uploads/cars/{map.ImagePath}"));
        }
    }
}
