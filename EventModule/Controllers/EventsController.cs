using System.ComponentModel.DataAnnotations;
using EventModule.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.EventModels;
using Shared.Extensions;

namespace EventModule.Controllers;

[Authorize]
public class EventsController : BaseController
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// Creates a new event based on the provided request data.
    /// </summary>
    /// <param name="model">The request data for creating the event.</param>
    /// <returns>
    /// - If the event is successfully created, it returns an HTTP 201 (Created) response with a link to the created event.
    /// - If there's an error in the creation process, it returns an appropriate error response.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> CreateEvent([Required] CreateEventRequest model)
    {
        var createEventResult = await _eventService.CreateEvent(model);
        return createEventResult.Match(
            eventObj => CreatedAtAction(nameof(GetEvent), routeValues: new { id = eventObj.Guid },
                createEventResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    /// <summary>
    /// Retrieves an event by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the event to retrieve.</param>
    /// <returns>
    /// - If the event is found, it returns an HTTP 200 (OK) response with an ApiResponse containing the event data.
    /// - If the event is not found or an error occurs, it returns an appropriate error response.
    /// </returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var getEventResult = await _eventService.GetEvent(id.ToString());

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return getEventResult.Match(
            _ => Ok(getEventResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }


    /// <summary>
    /// Updates an existing event with the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the event to update.</param>
    /// <param name="model">The request data for updating the event.</param>
    /// <returns>
    /// - If the event is successfully updated, it returns an HTTP 204 (No Content) response.
    /// - If there's an error in the update process, it returns an appropriate error response.
    /// </returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [Required] UpdateEventRequest model)
    {
        var updateEventResult = await _eventService.UpdateEvent(id.ToString(), model);
        return updateEventResult.Match(_ => NoContent(), ReturnErrorResponse);
    }

    /// <summary>
    /// Deletes an event with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the event to delete.</param>
    /// <returns>
    /// - If the event is successfully deleted, it returns an HTTP 204 (No Content) response.
    /// - If there's an error in the deletion process, it returns an appropriate error response.
    /// </returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var deleteEventResult = await _eventService.DeleteEvent(id.ToString());
        return deleteEventResult.Match(_ => NoContent(), ReturnErrorResponse);
    }
}