using AutoMapper;
using PhotoApp.Core.Models;
using PhotoApp.Infrastructure.Entities;

namespace PhotoApp.Api.Helper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<PhotoEntity, PhotoModel>();
        }
    }
}
