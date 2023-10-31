using System.ComponentModel.DataAnnotations;
using EventModule.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.API;
using Shared.Enums;
using Shared.EventModels.Requests;
using Shared.Extensions;

namespace EventModule.Controllers;

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
    [Authorize(Roles = UserRoleStrings.AdminAndUserRoles)]
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
    [Authorize(Roles = UserRoleStrings.AdminAndUserRoles)]
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
    [Authorize(Roles = UserRoleStrings.Admin)]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var deleteEventResult = await _eventService.DeleteEvent(id.ToString());
        return deleteEventResult.Match(_ => NoContent(), ReturnErrorResponse);
    }

    /// <summary>
    /// Searches for events based on the provided search criteria.
    /// </summary>
    /// <param name="request">The search criteria.</param>
    /// <param name="sort"></param>
    /// <returns>A list of events that match the search criteria.</returns>
    /// <remarks>Returns an empty list if no events match the criteria</remarks>
    [HttpGet("search")]
    public async Task<IActionResult> SearchEvents([FromQuery] SearchEventRequest request)
    {
        var searchEventResult = await _eventService.SearchEvents(request);
        return searchEventResult.Match(
            _ => Ok(searchEventResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }


    /// <summary>
    /// Gets events with available tickets.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A list of events that match the criteria.</returns>
    /// <remarks>Returns an empty list if no events match the criteria</remarks>
    [HttpGet("available-tickets")]
    public async Task<IActionResult> GetEventsWithAvailableTickets(int pageNumber = SearchConstants.PageNumber,
        int pageSize = SearchConstants.PageSize)
    {
        var searchEventResult = await _eventService.GetEventsWithAvailableTickets(pageNumber, pageSize);
        return searchEventResult.Match(
            _ => Ok(searchEventResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
    
    [HttpPost("{id:guid}/buy-ticket")]
    // todo: make this accept a json body or params to include "how many tickets are being bought"
    public async Task<IActionResult> BuyTickets(Guid id, TicketPurchaseRequest request)
    {
        var ticketPurchaseResult = await _eventService.BuyTicket(id.ToString());
        
        return ticketPurchaseResult.Match(
            _ => Ok(ticketPurchaseResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}