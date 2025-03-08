using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models.Images;

namespace EventsApp.BLL.MappingProfiles;

public class ImageFileProfile : Profile
{
    public ImageFileProfile()
    {
        CreateMap<ImageFileModel, ImageFileEntity>().ReverseMap();
    }
}