using APICourse.DTO;
using APICourse.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Identity.Client.Extensibility;

namespace APICourse.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegisterResponseDto>();
            CreateMap<User, RegisterResponseDto>();
            CreateMap<Cart,CartResponseDto>();
            CreateMap<CartItem,CartItemResponseDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(price => price.ProductPrice, price => price.MapFrom(price => price.Price))
                .ForMember(s=>s.ProductName,s=>s.MapFrom(s=>s.Name))
                .ForMember(s=>s.ProductStock,s=>s.MapFrom(s=>s.Stock));
            CreateMap<Order, OrderResponseDto>();
            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(
                    dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name)
                );

            CreateMap<Payment, PaymantResponseDto>();
        }
    }
}
