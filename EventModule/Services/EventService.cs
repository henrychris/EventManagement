using AutoMapper;
using EventModule.Data;
using EventModule.Data.Models;
using EventModule.Interfaces;
using Shared.Enums;
using Shared.EventModels;
using ErrorOr;
using EventModule.ServiceErrors;

namespace EventModule.Services;

public class EventService : IEventService
{
    private readonly EventDbContext _dbContext;
    private readonly IMapper _mapper;

    public EventService(EventDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ErrorOr<EventResponse>> CreateEvent(CreateEventRequest request)
    {
        var validateEventResult = ValidateEvent(request);
        if (validateEventResult.IsError)
        {
            return validateEventResult.Errors;
        }

        var newEvent = _mapper.Map<Event>(request);
        await _dbContext.Events.AddAsync(newEvent);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<EventResponse>(newEvent);
    }

    public async Task<ErrorOr<EventResponse>> GetEvent(string id)
    {
        var result = await _dbContext.Events.FindAsync(id);
        return result is not null ? _mapper.Map<EventResponse>(result) : Errors.Event.NotFound;
    }

    public async Task<ErrorOr<EventResponse>> UpdateEvent(string id, UpdateEventRequest request)
    {
        var existingEvent = await _dbContext.Events.FindAsync(id);
        if (existingEvent is null)
        {
            return Errors.Event.NotFound;
        }

        existingEvent.Name = string.IsNullOrEmpty(request.Name) ? existingEvent.Name : request.Name;
        existingEvent.Description =
            string.IsNullOrEmpty(request.Description) ? existingEvent.Description : request.Description;

        if (request.Price != null)
        {
            existingEvent.Price = request.Price.Value;
        }

        if (request.Date != null)
        {
            existingEvent.Date = request.Date.Value;
        }

        if (request.StartTime != null)
        {
            existingEvent.StartTime = request.StartTime.Value;
        }

        if (request.EndTime != null)
        {
            existingEvent.EndTime = request.EndTime.Value;
        }

        if (request.EventStatus != null)
        {
            if (Enum.TryParse(request.EventStatus, out EventStatus status))
            {
                existingEvent.EventStatus = status;
            }
            else
            {
                // Handle the case where the provided EventStatus is not a valid enum value.
                // store the enum string as in EventStatus
                throw new Exception("Invalid Event State.");
            }
        }

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<EventResponse>(existingEvent);
    }

    public async Task<ErrorOr<Deleted>> DeleteEvent(string id)
    {
        var entityToRemove = await _dbContext.Events.FindAsync(id);
        if (entityToRemove is null)
        {
            return Errors.Event.NotFound;
        }

        _dbContext.Events.Remove(entityToRemove);
        await _dbContext.SaveChangesAsync();
        return Result.Deleted;
    }

    private static ErrorOr<Event> ValidateEvent(CreateEventRequest request)
    {
        List<Error> errors = new();

        if (request.Date < DateTime.UtcNow)
        {
            errors.Add(Errors.Event.InvalidEventDate);
        }

        if (request.StartTime < request.Date)
        {
            errors.Add(Errors.Event.InvalidEventStartTime);
        }

        if (request.EndTime < request.Date || request.EndTime < request.StartTime)
        {
            errors.Add(Errors.Event.InvalidEventEndTime);
        }

        if (request.Price < 0)
        {
            errors.Add(Errors.Event.InvalidTicketPrice);
        }

        return errors;
    }
}