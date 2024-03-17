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
                .ForMember(dest => dest.Description, opts => opts.MapFrom(y => y.Description))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(y => y.Name))
                .ReverseMap();

            CreateMap<ProductDto, ProductStorage>().ReverseMap();

            CreateMap<Product, CategoryProduct>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.CategoryId, opts => opts.MapFrom(y => y.CategoryId))
                .ForMember(dest => dest.ProductId, opts => opts.MapFrom(y => y.Id))
                .ReverseMap();

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Name, opts => opts.MapFrom(y => y.Name))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(y => y.Description))
                .ForMember(dest => dest.CategoryId, opts => opts.MapFrom(y => y.CategoryId))
                .ReverseMap();

            CreateMap<UpdateProductDto, ProductStorage>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Name, opts => opts.MapFrom(y => y.Name))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(y => y.Description))
                .ForMember(dest => dest.CategoryId, opts => opts.MapFrom(y => y.CategoryId))
                .ReverseMap();
        }
    }
}