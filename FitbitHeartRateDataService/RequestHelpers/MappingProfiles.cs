using AutoMapper;
using Contracts;
using FitbitHeartRateDataService.DTOs;
using FitbitHeartRateDataService.Entities;

namespace FitbitHeartRateDataService.RequestHelpers;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // CreateMap<Source, Destination>
        CreateMap<CreateHeartRateDto, HeartRate>();
        CreateMap<HeartRate, HeartRateDto>();
        CreateMap<HeartRateDto, HeartRatesCreated>(); // Both the Fitbit and Search service know about the HeartRatesCreated contract, so we will use it
    }
}