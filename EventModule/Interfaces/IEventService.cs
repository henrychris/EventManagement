﻿using ErrorOr;
using Shared.EventModels.Requests;
using Shared.EventModels.Responses;

namespace EventModule.Interfaces;

public interface IEventService
{
    Task<ErrorOr<EventResponse>> CreateEvent(CreateEventRequest request);
    Task<ErrorOr<EventResponse>> GetEvent(string id);
    Task<ErrorOr<EventResponse>> UpdateEvent(string id, UpdateEventRequest request);
    Task<ErrorOr<Deleted>> DeleteEvent(string id);
}