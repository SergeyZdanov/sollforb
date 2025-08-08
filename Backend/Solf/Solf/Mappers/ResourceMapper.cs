using API.DTO.Resourse;
using AutoMapper;
using Database.Models;

namespace API.Mappers
{
    public class ResourceMapper : Profile
    {
        public ResourceMapper()
        {
            CreateMap<ResourceCreateDto, Resource>();
        }
    }
}
