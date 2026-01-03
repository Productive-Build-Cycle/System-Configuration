using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.API.Helpers;
using PBC.SystemConfiguration.Application.Dtos.AppSetting;
using PBC.SystemConfiguration.Application.Extensions;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Enums;

namespace PBC.SystemConfiguration.API.Controllers;

public class AppSettingsController(IAppSettingService appSettingService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await appSettingService.GetAllAsync(cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result);
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetById(string key, CancellationToken cancellationToken)
    {
        var result = await appSettingService.GetByKeyAsync(key, cancellationToken);
        if (result == null)
            return ResponseHelper.CreateErrorResponse<AppSettingDto>(404, "App Setting not found");

        return ResponseHelper.CreateSuccessResponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppSettingDto dto, CancellationToken cancellationToken)
    {
        var result = await appSettingService.CreateAsync(dto, cancellationToken);
        return ResponseHelper.CreateSuccessResponse(result, ResultEnum.CreatedSuccessfully.GetDescription());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAppSettingDto dto, CancellationToken cancellationToken)
    {
        var success = await appSettingService.UpdateAsync(id, dto, cancellationToken);
        if (!success)
            return ResponseHelper.CreateErrorResponse<AppSettingDto>(404, "App Setting not found");

        return ResponseHelper.CreateSuccessResponse<object>(null, ResultEnum.UpdatedSuccessfully.GetDescription());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var success = await appSettingService.DeleteAsync(id, cancellationToken);
        if (!success)
            return ResponseHelper.CreateErrorResponse<AppSettingDto>(404, "App Setting not found");

        return ResponseHelper.CreateSuccessResponse<object>(null, ResultEnum.DeletedSuccessfully.GetDescription());
    }
}