using Marketplace.Models;
using AutoMapper;

namespace Marketplace.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductCreateDTO, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore()) // игнорируем Category
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // игнорируем Id
        }
    }
}
