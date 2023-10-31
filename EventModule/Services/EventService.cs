using AutoMapper;
using EventModule.Data.Models;
using EventModule.Interfaces;
using Shared.Enums;
using ErrorOr;
using EventModule.Repositories;
using EventModule.ServiceErrors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.EventModels.Requests;
using Shared.EventModels.Responses;
using Shared.Extensions;

namespace EventModule.Services;

public class EventService : IEventService
{
    private readonly IMapper _mapper;
    private readonly IValidator<Event> _eventValidator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EventService> _logger;

    public EventService(IMapper mapper, IValidator<Event> eventValidator,
        IUnitOfWork unitOfWork, ILogger<EventService> logger)
    {
        _mapper = mapper;
        _eventValidator = eventValidator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<EventResponse>> CreateEvent(CreateEventRequest request)
    {
        var newEvent = _mapper.Map<Event>(request);

        var validateEventResult = await _eventValidator.ValidateAsync(newEvent);
        if (!validateEventResult.IsValid)
        {
            return validateEventResult.ToErrorList();
        }

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

        existingEvent.Name = request.Name ?? existingEvent.Name;
        existingEvent.Description = request.Description ?? existingEvent.Description;
        existingEvent.Price = request.Price ?? existingEvent.Price;
        existingEvent.Date = request.Date ?? existingEvent.Date;
        existingEvent.StartTime = request.StartTime ?? existingEvent.StartTime;
        existingEvent.EndTime = request.EndTime ?? existingEvent.EndTime;
        existingEvent.TicketsAvailable = request.TicketsAvailable ?? existingEvent.TicketsAvailable;

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

        var validationResult = await _eventValidator.ValidateAsync(existingEvent);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
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

    // todo: add integration tests
    // test all possible situations
    public async Task<ErrorOr<SearchEventResponse>> SearchEvents(SearchEventRequest request)
    {
        Enum.TryParse<EventSortOption>(request.Sort, true, out var sortOption);
        var result = await _unitOfWork.Events.SearchEvents(request, sortOption);
        var eventData = result.Select(x => _mapper.Map<EventResponse>(x)).ToList();
        return new SearchEventResponse(eventData, eventData.Count);
    }

    public async Task<ErrorOr<SearchEventResponse>> GetEventsWithAvailableTickets(int pageNumber, int pageSize,
        string sort)
    {
        Enum.TryParse<EventSortOption>(sort, true, out var sortOption);
        var result = await _unitOfWork.Events.GetEventsWithAvailableTickets(pageNumber, pageSize, sortOption);
        var eventData = result.Select(x => _mapper.Map<EventResponse>(x)).ToList();
        return new SearchEventResponse(eventData, eventData.Count);
    }

    public async Task<ErrorOr<TicketPurchaseResponse>> BuyTicket(string id)
    {
        var eventObj = await _unitOfWork.Events.GetByIdAsync(id);
        if (eventObj is null)
        {
            return Errors.Event.NotFound;
        }

        if (eventObj.TicketsAvailable == 0)
        {
            return Errors.Event.NoTicketsAvailable;
        }

        eventObj.TicketsAvailable--;
        if (eventObj.TicketsAvailable < 0)
        {
            _logger.LogError($"Event {eventObj.Id} would have negative tickets available.");
            throw new InvalidOperationException("Event has negative tickets available.");
        }

        try
        {
            _unitOfWork.Events.Update(eventObj);
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, Errors.Event.ConcurrencyConflict.Description);
            return Errors.Event.ConcurrencyConflict;
        }

        return new TicketPurchaseResponse(eventObj.Name);
    }
}