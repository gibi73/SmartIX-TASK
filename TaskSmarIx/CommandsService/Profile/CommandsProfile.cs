using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;
//using PlatformService;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformreadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            
            CreateMap<GrpcPlatformModel, Platform>()
                 .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                 
        }
    }
}