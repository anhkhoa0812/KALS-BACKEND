using AutoMapper;
using KALS.API.Models.GoogleDrive;
using KALS.API.Models.Lab;
using KALS.Domain.Entities;

namespace KALS.API.Mapper;

public class LabMapper: Profile
{
    public LabMapper()
    {
        CreateMap<CreateLabRequest, Lab>();
        CreateMap<Lab, LabResponse>();
    }
}