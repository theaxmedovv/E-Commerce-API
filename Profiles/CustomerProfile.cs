using AutoMapper;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs;
using AtirAPI.Models;

namespace ECommerceAPI.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerCreateDTO, Customer>();
            CreateMap<Customer, CustomerDTO>();
        }
    }
}