using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    /// <summary>
    /// Controller quản lý Vaccine - kế thừa BaseController để tận dụng CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VaccineController : BaseController<IVaccineService, VaccineDto>
    {
        private readonly IVaccineService _vaccineService;

        public VaccineController(IVaccineService vaccineService) : base(vaccineService)
        {
            _vaccineService = vaccineService;
        }

        /// <summary>
        /// Lấy danh sách tất cả vaccine
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var vaccines = await _vaccineService.GetAllAsync();
                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách vaccine thành công",
                    data = vaccines
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
        /// Lấy thông tin vaccine theo ID - Override từ BaseController
        /// </summary>
        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var vaccine = await _vaccineService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Lấy thông tin vaccine thành công",
                    data = vaccine
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
        /// Tạo vaccine mới - Sử dụng CreateVaccineDto
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateVaccineDto dto)
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

                var vaccine = await _vaccineService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = vaccine.Id }, new
                {
                    success = true,
                    message = "Tạo vaccine thành công",
                    data = vaccine
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
        /// Cập nhật vaccine - Sử dụng UpdateVaccineDto
        /// </summary>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVaccineDto dto)
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

                var vaccine = await _vaccineService.UpdateAsync(id, dto);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật vaccine thành công",
                    data = vaccine
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
        /// Xóa vaccine - Override từ BaseController
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _vaccineService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Xóa vaccine thành công"
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
