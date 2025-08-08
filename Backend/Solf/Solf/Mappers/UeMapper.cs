
using API.DTO.UE;
using AutoMapper;
using Database.Models;

namespace API.Mappers
{
    public class UeMapper : Profile
    {
        public UeMapper()
        {
            CreateMap<UeCreateDto, UE>();
        }
    }
}
