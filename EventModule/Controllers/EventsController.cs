using EventModule.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.EventModels;
using Shared.Extensions;

namespace EventModule.Controllers;

public class EventsController : BaseController
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(CreateEventRequest model)
    {
        var response = await _eventService.CreateEvent(model);
        return CreatedAtAction(nameof(GetEvent), routeValues: new { id = response.Guid }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var getEventResult = await _eventService.GetEvent(id.ToString());

        return getEventResult.Match(
            _ => Ok(getEventResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEvent(Guid id, UpdateEventRequest model)
    {
        var result = await _eventService.UpdateEvent(id.ToString(), model);
        // todo: create a serviceResponse object mkay?
        return result is null ? BadRequest() : Ok(model);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var isDeleted = await _eventService.DeleteEvent(id.ToString());
        // 204 is returned when the object to be deleted doesn't even exist.
        return isDeleted ? Ok() : NoContent();
    }
}