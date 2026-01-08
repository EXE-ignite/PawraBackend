using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VaccinationRecordController : ControllerBase
    {
        private readonly IVaccinationRecordService _vaccinationRecordService;

        public VaccinationRecordController(IVaccinationRecordService vaccinationRecordService)
        {
            _vaccinationRecordService = vaccinationRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _vaccinationRecordService.GetAllAsync();
                return Ok(new { success = true, message = "Lấy danh sách lịch sử tiêm chủng thành công", data = records });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var record = await _vaccinationRecordService.GetByIdAsync(id);
                return Ok(new { success = true, message = "Lấy thông tin lịch sử tiêm chủng thành công", data = record });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVaccinationRecordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var record = await _vaccinationRecordService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = record.Id }, new { success = true, message = "Tạo lịch sử tiêm chủng thành công", data = record });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVaccinationRecordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var record = await _vaccinationRecordService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Cập nhật lịch sử tiêm chủng thành công", data = record });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _vaccinationRecordService.DeleteAsync(id);
                return Ok(new { success = true, message = "Xóa lịch sử tiêm chủng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
