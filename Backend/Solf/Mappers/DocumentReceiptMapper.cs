using API.DTO.DocumentReceipt;
using AutoMapper;
using Database.Models;

namespace API.Mappers
{
    public class DocumentReceiptMapper : Profile
    {
        public DocumentReceiptMapper()
        {
            CreateMap<ReceiptResourceDto, ResourceReceipt>()
                .ForMember(dest => dest.UE_Id, opt => opt.MapFrom(src => src.UnitId));

            CreateMap<DocumentReceiptCreateDto, DocumentReceipt>()
                .ForMember(dest => dest.ResourceReceipts, opt => opt.MapFrom(src => src.Resources));

            CreateMap<ResourceReceipt, ReceiptResourceResponseDto>()
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UE_Id))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Ue.Name))
                .ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.Resource.Name));

            CreateMap<DocumentReceipt, DocumentReceiptDto>()
                .ForMember(dest => dest.Resources, opt => opt.MapFrom(src => src.ResourceReceipts));
        }
    }
}