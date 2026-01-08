using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    /// <summary>
    /// Controller quản lý ClinicService - kế thừa BaseController để tận dụng CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClinicServiceController : BaseController<IClinicServiceService, ClinicServiceDto>
    {
        private readonly IClinicServiceService _clinicServiceService;

        public ClinicServiceController(IClinicServiceService clinicServiceService) : base(clinicServiceService)
        {
            _clinicServiceService = clinicServiceService;
        }

        /// <summary>
        /// Lấy danh sách tất cả dịch vụ phòng khám
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clinicServices = await _clinicServiceService.GetAllAsync();
                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách dịch vụ phòng khám thành công",
                    data = clinicServices
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
        /// Lấy thông tin dịch vụ phòng khám theo ID - Override từ BaseController
        /// </summary>
        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var clinicService = await _clinicServiceService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Lấy thông tin dịch vụ phòng khám thành công",
                    data = clinicService
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
        /// Tạo dịch vụ phòng khám mới - Sử dụng CreateClinicServiceDto
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateClinicServiceDto dto)
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

                var clinicService = await _clinicServiceService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = clinicService.Id }, new
                {
                    success = true,
                    message = "Tạo dịch vụ phòng khám thành công",
                    data = clinicService
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
        /// Cập nhật dịch vụ phòng khám - Sử dụng UpdateClinicServiceDto
        /// </summary>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClinicServiceDto dto)
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

                var clinicService = await _clinicServiceService.UpdateAsync(id, dto);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật dịch vụ phòng khám thành công",
                    data = clinicService
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
        /// Xóa dịch vụ phòng khám - Override từ BaseController
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _clinicServiceService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Xóa dịch vụ phòng khám thành công"
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
