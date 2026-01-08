using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    /// <summary>
    /// Controller quản lý ClinicManager - kế thừa BaseController để tận dụng CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClinicManagerController : BaseController<IClinicManagerService, ClinicManagerDto>
    {
        private readonly IClinicManagerService _clinicManagerService;

        public ClinicManagerController(IClinicManagerService clinicManagerService) : base(clinicManagerService)
        {
            _clinicManagerService = clinicManagerService;
        }

        /// <summary>
        /// Lấy danh sách tất cả quản lý phòng khám
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var managers = await _clinicManagerService.GetAllAsync();
                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách quản lý phòng khám thành công",
                    data = managers
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
        /// Lấy thông tin quản lý phòng khám theo ID - Override từ BaseController
        /// </summary>
        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var manager = await _clinicManagerService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Lấy thông tin quản lý phòng khám thành công",
                    data = manager
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
        /// Tạo quản lý phòng khám mới - Sử dụng CreateClinicManagerDto
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateClinicManagerDto dto)
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

                var manager = await _clinicManagerService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = manager.Id }, new
                {
                    success = true,
                    message = "Tạo quản lý phòng khám thành công",
                    data = manager
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
        /// Cập nhật quản lý phòng khám - Sử dụng UpdateClinicManagerDto
        /// </summary>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClinicManagerDto dto)
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

                var manager = await _clinicManagerService.UpdateAsync(id, dto);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật quản lý phòng khám thành công",
                    data = manager
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
        /// Xóa quản lý phòng khám - Override từ BaseController
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _clinicManagerService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Xóa quản lý phòng khám thành công"
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
