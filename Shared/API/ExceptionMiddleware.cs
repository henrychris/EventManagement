﻿using System.Net;
using Shared.Responses;

namespace Shared.API;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        // can catch specific exceptions here.
        catch (Exception ex)
        {
            LogException(httpContext, ex);
            await HandleExceptionAsync(httpContext, ex.Message);
        }
    }

    private void LogException(HttpContext httpContext, Exception ex)
    {
        var http = httpContext.GetEndpoint()?.DisplayName?.Split(" => ")[0] ?? httpContext.Request.Path.ToString();
        var httpMethod = httpContext.Request.Method;
        var type = ex.GetType().Name;
        var error = ex.Message;
        var msg =
            $"""
             Something went wrong.
             =================================
             ENDPOINT: {http}
             METHOD: {httpMethod}
             TYPE: {type}
             REASON: {error}
             ---------------------------------
             {ex.StackTrace}
             """;
        _logger.LogError("{@msg}", msg);
    }

    private static async Task HandleExceptionAsync(HttpContext context, string errorMessage)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ApiResponse<object>(null, "Sorry, something went wrong!", false)
        {
            Note = string.Concat(errorMessage, " ", "Check the application logs for more info.")
        };
        await context.Response.WriteAsync(response.ToJsonString());
    }
}