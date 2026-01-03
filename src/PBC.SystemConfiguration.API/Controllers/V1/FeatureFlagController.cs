using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.API.Helpers;
using PBC.SystemConfiguration.Application.Dtos.Common;
using PBC.SystemConfiguration.Application.Dtos.FeatureFlag;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Enums;
using PBC.SystemConfiguration.Domain.Extensions;

namespace PBC.SystemConfiguration.API.Controllers.V1;

/// <summary>
/// Exposes RESTful API endpoints for managing Feature Flags within the system.
/// </summary>
/// <remarks>
/// This controller is responsible for handling HTTP requests related to Feature Flag
/// operations such as retrieval, creation, update, and deletion. It delegates all
/// business logic to the <see cref="IFeatureFlagService"/> and focuses solely on
/// request handling, response formatting, and HTTP concerns.
///
/// API versioning is applied via the controller namespace (V1), ensuring backward
/// compatibility as the system evolves. All responses are standardized using
/// <see cref="ResponseHelper"/> to provide consistent API contracts.
/// </remarks>
public class FeatureFlagController(IFeatureFlagService featureFlagService) : BaseController
{
    /// <summary>
    /// Retrieves all feature flags.
    /// </summary>
    /// <remarks>
    /// Returns a list of all feature flags in the system.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of feature flags.</returns>
    /// <response code="200">Returns the list of feature flags.</response>
    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<FeatureFlagDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await featureFlagService.GetAllAsync(cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result);
    }

    /// <summary>
    /// Retrieves a specific feature flag by Name.
    /// </summary>
    /// <param name="name">The unique name of the feature flag.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The requested feature flag.</returns>
    /// <response code="200">Returns the feature flag.</response>
    /// <response code="404">If the feature flag is not found.</response>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(Response<FeatureFlagDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
    {
        var result = await featureFlagService.GetByNameAsync(name, cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result);
    }

    /// <summary>
    /// Creates a new feature flag.
    /// </summary>
    /// <param name="createDto">The feature flag creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created feature flag.</returns>
    /// <response code="201">Returns the created feature flag.</response>
    /// <response code="400">If the input is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Response<FeatureFlagDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateFeatureFlagDto createDto, CancellationToken cancellationToken)
    {
        var result = await featureFlagService.CreateAsync(createDto, cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result, ResultEnum.CreatedSuccessfully.GetDescription());
    }

    /// <summary>
    /// Updates an existing feature flag.
    /// </summary>
    /// <param name="id">The ID of the feature flag to update.</param>
    /// <param name="updateDto">The updated feature flag data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated feature flag.</returns>
    /// <response code="200">Returns the updated feature flag.</response>
    /// <response code="404">If the feature flag is not found.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Response<FeatureFlagDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateFeatureFlagDto updateDto, CancellationToken cancellationToken)
    {
        await featureFlagService.UpdateAsync(id, updateDto, cancellationToken);
        return ResponseHelper.CreateSuccessResponse<object>(null, ResultEnum.UpdatedSuccessfully.GetDescription());
    }

    /// <summary>
    /// Deletes a feature flag.
    /// </summary>
    /// <param name="id">The ID of the feature flag to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="200">Indicates successful deletion.</response>
    /// <response code="404">If the feature flag is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await featureFlagService.DeleteAsync(id, cancellationToken);
        return ResponseHelper.CreateSuccessResponse<object>(null, ResultEnum.DeletedSuccessfully.GetDescription());
    }
}