using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VeterinarianController : ControllerBase
    {
        private readonly IVeterinarianService _veterinarianService;

        public VeterinarianController(IVeterinarianService veterinarianService)
        {
            _veterinarianService = veterinarianService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var veterinarians = await _veterinarianService.GetAllAsync();
                return Ok(new { success = true, message = "Lấy danh sách bác sĩ thú y thành công", data = veterinarians });
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
                var veterinarian = await _veterinarianService.GetByIdAsync(id);
                return Ok(new { success = true, message = "Lấy thông tin bác sĩ thú y thành công", data = veterinarian });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVeterinarianDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var veterinarian = await _veterinarianService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = veterinarian.Id }, new { success = true, message = "Tạo bác sĩ thú y thành công", data = veterinarian });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVeterinarianDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var veterinarian = await _veterinarianService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Cập nhật bác sĩ thú y thành công", data = veterinarian });
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
                await _veterinarianService.DeleteAsync(id);
                return Ok(new { success = true, message = "Xóa bác sĩ thú y thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

