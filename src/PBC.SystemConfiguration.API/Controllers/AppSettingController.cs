using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.Domain.Entities;
using PBC.SystemConfiguration.Domain.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace PBC.SystemConfiguration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingController : ControllerBase
    {
        private readonly IAppSettingRepository _repository;

        /// <summary>
        /// سازنده کنترلر AppSetting و تزریق وابستگی Repository
        /// </summary>
        /// <param name="repository">رپازیتوری AppSetting</param>
        public AppSettingController(IAppSettingRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// دریافت همه AppSetting‌ها
        /// </summary>
        /// <returns>لیست تمام AppSetting‌ها</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppSetting>>> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// دریافت یک AppSetting با شناسه مشخص
        /// </summary>
        /// <param name="id">شناسه AppSetting</param>
        /// <returns>AppSetting مورد نظر یا NotFound اگر موجود نبود</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AppSetting>> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        /// <summary>
        /// ایجاد یک AppSetting جدید
        /// </summary>
        /// <param name="newItem">AppSetting جدید</param>
        /// <returns>AppSetting ایجاد شده به همراه آدرس آن</returns>
        [HttpPost]
        public async Task<ActionResult<AppSetting>> Create(AppSetting newItem)
        {
            await _repository.AddAsync(newItem);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        /// <summary>
        /// بروزرسانی AppSetting موجود
        /// </summary>
        /// <param name="id">شناسه AppSetting برای بروزرسانی</param>
        /// <param name="updatedItem">داده‌های جدید AppSetting</param>
        /// <returns>نتیجه عملیات بروزرسانی</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AppSetting updatedItem)
        {
            if (id != updatedItem.Id)
                return BadRequest("Id در مسیر و بدنه درخواست باید یکسان باشد.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            _repository.Update(updatedItem);
            return NoContent();
        }

        /// <summary>
        /// حذف یک AppSetting با شناسه مشخص
        /// </summary>
        /// <param name="id">شناسه AppSetting برای حذف</param>
        /// <returns>نتیجه عملیات حذف</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            _repository.Remove(existing);
            return NoContent();
        }
    }
}
