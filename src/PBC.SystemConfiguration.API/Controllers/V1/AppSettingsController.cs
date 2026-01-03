using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.API.Helpers;
using PBC.SystemConfiguration.Application.Dtos.AppSetting;
using PBC.SystemConfiguration.Application.Dtos.Common;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Enums;
using PBC.SystemConfiguration.Domain.Extensions;

namespace PBC.SystemConfiguration.API.Controllers.V1;

/// <summary>
/// Exposes RESTful API endpoints for managing application configuration settings.
/// </summary>
/// <remarks>
/// This controller provides CRUD operations for application-level settings that are
/// used to configure system behavior at runtime. It acts as a thin HTTP layer and
/// delegates all business logic to the <see cref="IAppSettingService"/>, ensuring a
/// clean separation of concerns.
///
/// The controller is versioned via the namespace (V1) to support backward compatibility.
/// All responses follow a consistent API contract using <see cref="ResponseHelper"/>.
/// </remarks>
public class AppSettingsController(IAppSettingService appSettingService) : BaseController
{
    /// <summary>
    /// Retrieves all application settings.
    /// </summary>
    /// <remarks>
    /// Returns a list of all configuration settings available in the system.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of app settings.</returns>
    /// <response code="200">Returns the list of app settings.</response>
    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<AppSettingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await appSettingService.GetAllAsync(cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result);
    }

    /// <summary>
    /// Retrieves a specific app setting by Key.
    /// </summary>
    /// <param name="key">The unique key of the app setting.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The requested app setting.</returns>
    /// <response code="200">Returns the app setting.</response>
    /// <response code="404">If the app setting is not found.</response>
    [HttpGet("{key}")]
    [ProducesResponseType(typeof(Response<AppSettingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string key, CancellationToken cancellationToken)
    {
        var result = await appSettingService.GetByKeyAsync(key, cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result);
    }

    /// <summary>
    /// Creates a new app setting.
    /// </summary>
    /// <param name="createDto">The app setting creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created app setting.</returns>
    /// <response code="201">Returns the created app setting.</response>
    /// <response code="400">If the input is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Response<AppSettingDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateAppSettingDto createDto, CancellationToken cancellationToken)
    {
        var result = await appSettingService.CreateAsync(createDto, cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result, ResultEnum.CreatedSuccessfully.GetDescription());
    }

    /// <summary>
    /// Updates an existing app setting.
    /// </summary>
    /// <param name="id">The ID of the app setting to update.</param>
    /// <param name="updateDto">The updated app setting data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated app setting.</returns>
    /// <response code="200">Returns the updated app setting.</response>
    /// <response code="404">If the app setting is not found.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Response<AppSettingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateAppSettingDto updateDto, CancellationToken cancellationToken)
    {
        await appSettingService.UpdateAsync(id, updateDto, cancellationToken);
        return ResponseHelper.CreateSuccessResponse<object>(null, ResultEnum.UpdatedSuccessfully.GetDescription());
    }

    /// <summary>
    /// Deletes an app setting.
    /// </summary>
    /// <param name="id">The ID of the app setting to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="200">Indicates successful deletion.</response>
    /// <response code="404">If the app setting is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await appSettingService.DeleteAsync(id, cancellationToken);
        return ResponseHelper.CreateSuccessResponse<object>(null, ResultEnum.DeletedSuccessfully.GetDescription());
    }
}