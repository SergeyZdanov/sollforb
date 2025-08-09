using API.DTO.Client;
using API.DTO.DocumentReceipt;
using AutoMapper;
using Database.Models;

namespace API.Mappers
{
    public class DocumentReceiptMapper : Profile 
    {
        public DocumentReceiptMapper() 
        {
            CreateMap<DocumentReceiptCreateDto, DocumentReceipt>();
        }
    }
}
