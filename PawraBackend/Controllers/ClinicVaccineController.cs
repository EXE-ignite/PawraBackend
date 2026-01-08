using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClinicVaccineController : ControllerBase
    {
        private readonly IClinicVaccineService _clinicVaccineService;

        public ClinicVaccineController(IClinicVaccineService clinicVaccineService)
        {
            _clinicVaccineService = clinicVaccineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clinicVaccines = await _clinicVaccineService.GetAllAsync();
                return Ok(new { success = true, message = "Lấy danh sách vaccine phòng khám thành công", data = clinicVaccines });
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
                var clinicVaccine = await _clinicVaccineService.GetByIdAsync(id);
                return Ok(new { success = true, message = "Lấy thông tin vaccine phòng khám thành công", data = clinicVaccine });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClinicVaccineDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var clinicVaccine = await _clinicVaccineService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = clinicVaccine.Id }, new { success = true, message = "Tạo vaccine phòng khám thành công", data = clinicVaccine });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClinicVaccineDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var clinicVaccine = await _clinicVaccineService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Cập nhật vaccine phòng khám thành công", data = clinicVaccine });
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
                await _clinicVaccineService.DeleteAsync(id);
                return Ok(new { success = true, message = "Xóa vaccine phòng khám thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
