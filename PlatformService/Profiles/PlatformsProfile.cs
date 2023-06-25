using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;
namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()   //class constructor
        {
            //Source --> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
        }
    }
}