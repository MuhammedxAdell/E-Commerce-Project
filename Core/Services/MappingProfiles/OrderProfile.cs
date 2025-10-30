using AutoMapper;
using Domain.Entities.OrderModule;
using Shared.Dtos.OrderModule;

namespace Services.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address , AddressDto>().ReverseMap();
            CreateMap<DeliveryMethod , DeliveryMethodResult>();
            CreateMap<OrderItem , OrderItemDto>()
                .ForMember(dest => dest.ProductId, options => options.MapFrom(s => s.Product.ProductId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(s => s.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(s => s.Product.PictureUrl));
        
            CreateMap<Order , OrderResult>()
                .ForMember(dest => dest.PaymentStatus, options => options.MapFrom(s => s.PaymentStatus.ToString()))
                .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total, options => options.MapFrom(s => s.SubTotal + s.DeliveryMethod.Price));


        }
    }
}
