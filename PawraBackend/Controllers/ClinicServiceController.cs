using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClinicServiceController : ControllerBase
    {
        private readonly IClinicServiceService _clinicServiceService;

        public ClinicServiceController(IClinicServiceService clinicServiceService)
        {
            _clinicServiceService = clinicServiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clinicServices = await _clinicServiceService.GetAllAsync();
                return Ok(new { success = true, message = "Lấy danh sách dịch vụ phòng khám thành công", data = clinicServices });
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
                var clinicService = await _clinicServiceService.GetByIdAsync(id);
                return Ok(new { success = true, message = "Lấy thông tin dịch vụ phòng khám thành công", data = clinicService });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClinicServiceDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var clinicService = await _clinicServiceService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = clinicService.Id }, new { success = true, message = "Tạo dịch vụ phòng khám thành công", data = clinicService });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClinicServiceDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var clinicService = await _clinicServiceService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Cập nhật dịch vụ phòng khám thành công", data = clinicService });
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
                await _clinicServiceService.DeleteAsync(id);
                return Ok(new { success = true, message = "Xóa dịch vụ phòng khám thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
