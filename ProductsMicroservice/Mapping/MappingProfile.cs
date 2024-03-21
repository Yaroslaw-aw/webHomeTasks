using AutoMapper;
using ProductsMicroservice.Db;
using ProductsMicroservice.DTO;

namespace ProductsMicroservice.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap <ClientDto, Client>()
                //.ForMember(dest => dest.Id, opts => opts.Ignore())
                //.ForMember(dest => dest.Name, opts => opts.MapFrom(y => y.Name))
                //.ForMember(dest => dest.Email, opts => opts.MapFrom(y => y.Email))
                .ReverseMap();

            CreateMap<ClientDto, GetClientsDto>()
                //.ForMember(dest => dest.Name, opts => opts.MapFrom(y => y.Name))
                //.ForMember(dest => dest.Email, opts => opts.MapFrom(y => y.Email))
                //.ForMember(dest => dest.StorageName, opts => opts.Ignore())
                //.ForMember(dest => dest.Count, opts => opts.Ignore())
                //.ForMember(dest => dest.Price, opts => opts.Ignore())
                .ReverseMap();
        }
    }
}
