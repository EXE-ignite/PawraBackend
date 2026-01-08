using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClinicManagerController : ControllerBase
    {
        private readonly IClinicManagerService _clinicManagerService;

        public ClinicManagerController(IClinicManagerService clinicManagerService)
        {
            _clinicManagerService = clinicManagerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var managers = await _clinicManagerService.GetAllAsync();
                return Ok(new { success = true, message = "Lấy danh sách quản lý phòng khám thành công", data = managers });
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
                var manager = await _clinicManagerService.GetByIdAsync(id);
                return Ok(new { success = true, message = "Lấy thông tin quản lý phòng khám thành công", data = manager });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClinicManagerDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var manager = await _clinicManagerService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = manager.Id }, new { success = true, message = "Tạo quản lý phòng khám thành công", data = manager });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClinicManagerDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var manager = await _clinicManagerService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Cập nhật quản lý phòng khám thành công", data = manager });
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
                await _clinicManagerService.DeleteAsync(id);
                return Ok(new { success = true, message = "Xóa quản lý phòng khám thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

