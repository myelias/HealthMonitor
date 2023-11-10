using AutoMapper;

namespace SearchService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<HeartRateCreated, HeartRateDate>();
    }

}