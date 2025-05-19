using AutoMapper;
using ECommerceAPI.Models; // Changed from AtirAPI.Models
using ECommerceAPI.DTOs;
using AtirAPI.DTOs;
using AtirAPI.Models;  // Changed from AtirAPI.DTOs

namespace ECommerceAPI.Profiles // Changed from AtirAPI.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductCreateDTO, Product>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl)); // Explicit ImageUrl mapping

            CreateMap<Product, ProductDTO>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();
        }
    }
}