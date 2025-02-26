using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models.Images;

namespace EventsApp.DAL.MappingProfiles.Images;

public class ImageFileProfile : Profile
{
    public ImageFileProfile()
    {
        CreateMap<ImageFileModel, ImageFileEntity>().ReverseMap();
    }
}