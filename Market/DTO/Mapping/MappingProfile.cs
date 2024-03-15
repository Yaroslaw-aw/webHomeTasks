using AutoMapper;
using Market.Models;

namespace Market.DTO.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<StorageDto, Storage>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<Product, ProductStorage>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.ProductId, opts => opts.MapFrom(y => y.Id))
                .ForMember(dest => dest.CategoryId, opts => opts.MapFrom(y => y.CategoryId))
                .ForMember(dest => dest.StorageId, opts => opts.MapFrom(y => y.StorageId))
                .ForMember(dest => dest.Count, opts => opts.MapFrom(y => y.Count))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(y => y.Description))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(y => y.Name))
                .ReverseMap();
        }
    }
}
