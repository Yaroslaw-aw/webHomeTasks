﻿using AutoMapper;
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
        }
    }
}