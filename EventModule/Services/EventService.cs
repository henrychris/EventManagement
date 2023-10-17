﻿using AutoMapper;
using EventModule.Data;
using EventModule.Data.Models;
using EventModule.Interfaces;
using Shared.Enums;
using Shared.EventModels;

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

    public async Task<EventResponse> CreateEvent(CreateEventRequest request)
    {
        var newEvent = _mapper.Map<Event>(request);
        // todo: validate event before mapping? or after. sha validate it.
        await _dbContext.Events.AddAsync(newEvent);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<EventResponse>(newEvent);
    }

    public async Task<EventResponse?> GetEvent(string id)
    {
        var result = await _dbContext.Events.FindAsync(id);
        return result is not null ? _mapper.Map<EventResponse>(result) : null;
    }

    public async Task<EventResponse?> UpdateEvent(string id, UpdateEventRequest request)
    {
        var existingEvent = await _dbContext.Events.FindAsync(id);
        if (existingEvent is null)
        {
            return null;
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

    public async Task<bool> DeleteEvent(string id)
    {
        // todo: return an actual response
        var entityToRemove = await _dbContext.Events.FindAsync(id);
        if (entityToRemove is null)
        {
            return false;
        }

        _dbContext.Events.Remove(entityToRemove);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}