using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.Application.Dtos.Common;
using PBC.SystemConfiguration.Domain.Enums;
using PBC.SystemConfiguration.Domain.Extensions;

namespace PBC.SystemConfiguration.API.Helpers;

/// <summary>
/// Helper class for creating consistent API responses.
/// </summary>
public static class ResponseHelper
{
    /// <summary>
    /// Creates a standardized API response with the given parameters.
    /// </summary>
    /// <typeparam name="T">The type of the data included in the response.</typeparam>
    /// <param name="statusCode">HTTP status code for the response.</param>
    /// <param name="message">message describing the result.</param>
    /// <param name="isSuccess">Indicates whether the response represents a successful operation.</param>
    /// <param name="data">The data to include in the response, if any.</param>
    /// <returns>An IActionResult containing the response.</returns>
    public static IActionResult CreateResponse<T>(
        int statusCode,
        string message,
        bool isSuccess,
        T? data = default
    )
    {
        var response = new Response<T>(statusCode, message, isSuccess, data);
        return new ObjectResult(response) { StatusCode = statusCode };
    }

    /// <summary>
    /// Creates a success response with the provided data and optional messages.
    /// </summary>
    /// <typeparam name="T">The type of the data included in the response.</typeparam>
    /// <param name="data">The data to include in the response.</param>
    /// <param name="message">message describing the success.</param>
    /// <returns>An IActionResult containing the success response.</returns>
    public static IActionResult CreateSuccessResponse<T>(
        T? data,
        string? message = null
    )
    {
        message ??= ResultEnum.Success.GetDescription();
        return CreateResponse(200, message, true, data);
    }

    /// <summary>
    /// Creates an error response with the provided status code and messages.
    /// </summary>
    /// <typeparam name="T">The type of the data (usually null in case of errors).</typeparam>
    /// <param name="statusCode">HTTP status code representing the error.</param>
    /// <param name="message">describing the error.</param>
    /// <returns>An IActionResult containing the error response.</returns>
    public static IActionResult CreateErrorResponse<T>(
        int statusCode,
        string message
    )
    {
        return CreateResponse<T>(statusCode, message, false);
    }
}