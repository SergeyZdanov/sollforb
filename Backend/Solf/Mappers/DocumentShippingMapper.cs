using API.DTO.DocumentShipping;
using AutoMapper;
using Database.Models;

namespace API.Mappers
{
    public class DocumentShippingMapper : Profile
    {
        public DocumentShippingMapper()
        {
            CreateMap<ShippingResourceDto, ResourceShipment>()
                .ForMember(dest => dest.UE_Id, opt => opt.MapFrom(src => src.UnitId));

            CreateMap<DocumentShippingCreateDto, DocumentShipping>()
                .ForMember(dest => dest.ResourceShipments, opt => opt.MapFrom(src => src.Resources));

            CreateMap<ResourceShipment, ShippingResourceResponseDto>()
               .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UE_Id))
               .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Ue.Name))
               .ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.Resource.Name));

            CreateMap<DocumentShipping, DocumentShippingDto>()
                .ForMember(dest => dest.Resources, opt => opt.MapFrom(src => src.ResourceShipments));
        }
    }
}