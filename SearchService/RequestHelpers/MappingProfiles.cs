using Amazon.Util;
using AutoMapper;
using Contracts;

namespace SearchService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<HeartRatesCreated, HeartRateDate>();
    }
}