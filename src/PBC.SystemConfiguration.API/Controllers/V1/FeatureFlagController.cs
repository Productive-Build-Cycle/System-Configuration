using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.API.Helpers;
using PBC.SystemConfiguration.Application.Dtos.FeatureFlag;
using PBC.SystemConfiguration.Application.Extensions;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Enums;

namespace PBC.SystemConfiguration.API.Controllers.V1;

public class FeatureFlagController(IFeatureFlagService featureFlagService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await featureFlagService.GetAllAsync(cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetById(string name, CancellationToken cancellationToken)
    {
        var result = await featureFlagService.GetByNameAsync(name, cancellationToken);
        if (result == null)
            return ResponseHelper.CreateErrorResponse<FeatureFlagDto>(404, "Feature Flag not found");

        return ResponseHelper.CreateSuccessResponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFeatureFlagDto dto, CancellationToken cancellationToken)
    {
        var result = await featureFlagService.CreateAsync(dto, cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result, ResultEnum.CreatedSuccessfully.GetDescription());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFeatureFlagDto dto, CancellationToken cancellationToken)
    {
        var success = await featureFlagService.UpdateAsync(id, dto, cancellationToken);
        if (!success)
            return ResponseHelper.CreateErrorResponse<FeatureFlagDto>(404, "Feature Flag not found");

        return ResponseHelper.CreateSuccessResponse<object>(null, ResultEnum.UpdatedSuccessfully.GetDescription());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var success = await featureFlagService.DeleteAsync(id, cancellationToken);
        if (!success)
            return ResponseHelper.CreateErrorResponse<FeatureFlagDto>(404, "Feature Flag not found");

        return ResponseHelper.CreateSuccessResponse<object>(null, ResultEnum.DeletedSuccessfully.GetDescription());
    }
}