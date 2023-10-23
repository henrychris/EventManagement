using AutoMapper;
using EventModule.Data.Models;
using EventModule.Interfaces;
using Shared.Enums;
using ErrorOr;
using EventModule.Repositories;
using EventModule.ServiceErrors;
using FluentValidation;
using Shared.EventModels.Requests;
using Shared.EventModels.Responses;
using Shared.Extensions;

namespace EventModule.Services;

public class EventService : IEventService
{
    private readonly IMapper _mapper;
    private readonly IValidator<CreateEventRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public EventService(IMapper mapper, IValidator<CreateEventRequest> validator,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<EventResponse>> CreateEvent(CreateEventRequest request)
    {
        var validateEventResult = await _validator.ValidateAsync(request);
        if (!validateEventResult.IsValid)
        {
            return validateEventResult.ToErrorList();
        }

        var newEvent = _mapper.Map<Event>(request);
        await _unitOfWork.Events.AddAsync(newEvent);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<EventResponse>(newEvent);
    }

    public async Task<ErrorOr<EventResponse>> GetEvent(string id)
    {
        var result = await _unitOfWork.Events.GetByIdAsync(id);
        return result is not null ? _mapper.Map<EventResponse>(result) : Errors.Event.NotFound;
    }

    public async Task<ErrorOr<EventResponse>> UpdateEvent(string id, UpdateEventRequest request)
    {
        var existingEvent = await _unitOfWork.Events.GetByIdAsync(id);
        if (existingEvent is null)
        {
            return Errors.Event.NotFound;
        }

        // todo: make all the changes, and validate the final Event object. then return errors if something is iffy.
        var errors = new List<Error>();
        if (!string.IsNullOrEmpty(request.Name))
        {
            existingEvent.Name = request.Name;
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            existingEvent.Description = request.Description;
        }

        if (request.Price.HasValue)
        {
            if (request.Price < 0)
            {
                errors.Add(Errors.Event.InvalidTicketPrice);
            }
            else
            {
                existingEvent.Price = request.Price.Value;
            }
        }

        if (request.Date.HasValue)
        {
            if (request.Date < DateTime.UtcNow)
            {
                errors.Add(Errors.Event.InvalidEventDate);
            }
            else
            {
                existingEvent.Date = request.Date.Value;
            }
        }

        if (request.StartTime.HasValue)
        {
            if (request.StartTime < existingEvent.Date)
            {
                errors.Add(Errors.Event.InvalidEventStartTime);
            }
            else
            {
                existingEvent.StartTime = request.StartTime.Value;
            }
        }

        if (request.EndTime.HasValue)
        {
            if (request.EndTime < existingEvent.Date || request.EndTime < existingEvent.StartTime)
            {
                errors.Add(Errors.Event.InvalidEventEndTime);
            }
            else
            {
                existingEvent.EndTime = request.EndTime.Value;
            }
        }

        if (!string.IsNullOrEmpty(request.EventStatus))
        {
            if (Enum.TryParse(request.EventStatus, out EventStatus status))
            {
                existingEvent.EventStatus = status;
            }
            else
            {
                throw new Exception("Invalid Event State.");
            }
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        _unitOfWork.Events.Update(existingEvent);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<EventResponse>(existingEvent);
    }

    public async Task<ErrorOr<Deleted>> DeleteEvent(string id)
    {
        var entityToRemove = await _unitOfWork.Events.GetByIdAsync(id);
        if (entityToRemove is null)
        {
            return Errors.Event.NotFound;
        }

        _unitOfWork.Events.Remove(entityToRemove);
        await _unitOfWork.CompleteAsync();
        return Result.Deleted;
    }
}