using AutoMapper;
using EventModule.Data.Models;
using Shared.EventModels;
using Shared.EventModels.Requests;
using Shared.EventModels.Responses;
using Shared.Extensions;

namespace EventModule;

public class EventMappingProfile : Profile
{
    public EventMappingProfile()
    {
        CreateMap<CreateEventRequest, Event>().ReverseMap();
        CreateMap<Event, EventResponse>()
            .MapRecordMember(a => a.Name, src => src.Name)
            .MapRecordMember(a => a.Description, src => src.Description)
            .MapRecordMember(a => a.Guid, src => src.Id)
            .MapRecordMember(a => a.Date, src => src.Date)
            .MapRecordMember(a => a.StartTime, src => src.StartTime)
            .MapRecordMember(a => a.EndTime, src => src.EndTime)
            .MapRecordMember(a => a.Price, src => src.Price)
            .MapRecordMember(a => a.EventStatus, src => src.EventStatus);
    }
}