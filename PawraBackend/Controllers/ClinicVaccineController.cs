using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    /// <summary>
    /// Controller quản lý ClinicVaccine - kế thừa BaseController để tận dụng CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClinicVaccineController : BaseController<IClinicVaccineService, ClinicVaccineDto>
    {
        private readonly IClinicVaccineService _clinicVaccineService;

        public ClinicVaccineController(IClinicVaccineService clinicVaccineService) : base(clinicVaccineService)
        {
            _clinicVaccineService = clinicVaccineService;
        }

        /// <summary>
        /// Lấy danh sách tất cả vaccine phòng khám
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clinicVaccines = await _clinicVaccineService.GetAllAsync();
                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách vaccine phòng khám thành công",
                    data = clinicVaccines
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
        /// Lấy thông tin vaccine phòng khám theo ID - Override từ BaseController
        /// </summary>
        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var clinicVaccine = await _clinicVaccineService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Lấy thông tin vaccine phòng khám thành công",
                    data = clinicVaccine
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
        /// Tạo vaccine phòng khám mới - Sử dụng CreateClinicVaccineDto
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateClinicVaccineDto dto)
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

                var clinicVaccine = await _clinicVaccineService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = clinicVaccine.Id }, new
                {
                    success = true,
                    message = "Tạo vaccine phòng khám thành công",
                    data = clinicVaccine
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
        /// Cập nhật vaccine phòng khám - Sử dụng UpdateClinicVaccineDto
        /// </summary>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClinicVaccineDto dto)
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

                var clinicVaccine = await _clinicVaccineService.UpdateAsync(id, dto);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật vaccine phòng khám thành công",
                    data = clinicVaccine
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
        /// Xóa vaccine phòng khám - Override từ BaseController
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _clinicVaccineService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Xóa vaccine phòng khám thành công"
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
