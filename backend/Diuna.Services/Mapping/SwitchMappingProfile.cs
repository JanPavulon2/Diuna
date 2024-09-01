using AutoMapper;
using Diuna.Models.Config;
using Diuna.Models.State;
using Diuna.Services.Switch;

namespace Diuna.Services.Mapping;

public class SwitchMappingProfile : Profile
{
    public SwitchMappingProfile()
    {
        CreateMap<SwitchConfig, SwitchControl>();
        CreateMap<SwitchState, SwitchControl>()
            .ForMember(dest => dest.IsOn, opt => opt.MapFrom(src => src.IsOn));
    }
}
