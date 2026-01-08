using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    /// <summary>
    /// Controller quản lý Veterinarian - kế thừa BaseController để tận dụng CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VeterinarianController : BaseController<IVeterinarianService, VeterinarianDto>
    {
        private readonly IVeterinarianService _veterinarianService;

        public VeterinarianController(IVeterinarianService veterinarianService) : base(veterinarianService)
        {
            _veterinarianService = veterinarianService;
        }

        /// <summary>
        /// Lấy danh sách tất cả bác sĩ thú y
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var veterinarians = await _veterinarianService.GetAllAsync();
                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách bác sĩ thú y thành công",
                    data = veterinarians
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy thông tin bác sĩ thú y theo ID - Override từ BaseController
        /// </summary>
        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var veterinarian = await _veterinarianService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Lấy thông tin bác sĩ thú y thành công",
                    data = veterinarian
                });
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Tạo bác sĩ thú y mới - Sử dụng CreateVeterinarianDto
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateVeterinarianDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Dữ liệu không hợp lệ",
                        errors = ModelState
                    });
                }

                var veterinarian = await _veterinarianService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = veterinarian.Id }, new
                {
                    success = true,
                    message = "Tạo bác sĩ thú y thành công",
                    data = veterinarian
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Cập nhật bác sĩ thú y - Sử dụng UpdateVeterinarianDto
        /// </summary>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVeterinarianDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Dữ liệu không hợp lệ",
                        errors = ModelState
                    });
                }

                var veterinarian = await _veterinarianService.UpdateAsync(id, dto);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật bác sĩ thú y thành công",
                    data = veterinarian
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Xóa bác sĩ thú y - Override từ BaseController
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _veterinarianService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Xóa bác sĩ thú y thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
