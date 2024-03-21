using ApiClientMarket.Db;
using AutoMapper;

namespace ApiClientMarket.Dto.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClientProductDto, ClientProduct>().ReverseMap();
        }
    }
}
