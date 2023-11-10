using AutoMapper;
using FitbitHeartRateDataService.DTOs;
using FitbitHeartRateDataService.Entities;

namespace FitbitHeartRateDataService.RequestHelpers;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<HeartRate, HeartRateDto>();
        CreateMap<CreateHeartRateDto, HeartRate>();
        CreateMap<HeartRateDto, HeartRateCreated>();
    }
}