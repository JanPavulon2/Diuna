using AutoMapper;
using Diuna.Services.Switch;

namespace Diuna.Services.Mapping;

public class SwitchMappingProfile : Profile
{
    public SwitchMappingProfile()
    {
        CreateMap<SwitchConfig, SwitchControl>()
            .ForMember(dest => dest.IsOn, opt => opt.Ignore());
    }
}
