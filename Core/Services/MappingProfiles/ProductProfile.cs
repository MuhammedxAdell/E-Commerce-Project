using AutoMapper;
using Domain.Entities.ProductModule;
using Shared.Dtos;

namespace Services.MappingProfiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductType, TypeResultDto>();
            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<Product, ProductResultDto>(). //Images ==> images/imageName.png [We need the BaseUrl before it] ==> https://localhost:5001/images/imageName.png
                ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.ProductType.Name)).
                ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PicturetUrlResolver>());
        }
    }
}
