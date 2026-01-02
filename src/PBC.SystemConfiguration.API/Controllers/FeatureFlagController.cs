using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Application.Dtos;

[ApiController]
[Route("api/feature-flags")]
public class FeatureFlagsController : ControllerBase
{
    private readonly IFeatureFlagService _featureFlagService;

    public FeatureFlagsController(IFeatureFlagService featureFlagService)
    {
        _featureFlagService = featureFlagService;
    }

    // GET /api/feature-flags
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _featureFlagService.GetAllAsync();
        return Ok(result);
    }

    // GET /api/feature-flags/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _featureFlagService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    // POST /api/feature-flags
    [HttpPost]
    public async Task<IActionResult> Create(CreateFeatureFlagDto dto)
    {
        var result = await _featureFlagService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT /api/feature-flags/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateFeatureFlagDto dto)
    {
        var result = await _featureFlagService.UpdateAsync(id, dto);
        return result == null ? NotFound() : Ok(result);
    }

    // DELETE /api/feature-flags/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _featureFlagService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
