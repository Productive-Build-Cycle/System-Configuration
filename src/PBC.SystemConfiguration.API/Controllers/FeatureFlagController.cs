using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Entities;

[ApiController]
[Route("api/feature-flags")]
public class FeatureFlagsController : ControllerBase
{
    private readonly IFeatureFlagService _featureFlagService;

    public FeatureFlagsController(IFeatureFlagService featureFlagService)
    {
        _featureFlagService = featureFlagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _featureFlagService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid Id)
    {
        var result = await _featureFlagService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FeatureFlag featureFlag)
    {
        await _featureFlagService.CreateAsync(featureFlag);
        return CreatedAtAction(nameof(GetById), new { id = featureFlag.Id }, featureFlag);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid Id, FeatureFlag featureFlag)
    {
        await _featureFlagService.UpdateAsync(Id, featureFlag);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        await _featureFlagService.DeleteAsync(Id);
        return NoContent();
    }
}
