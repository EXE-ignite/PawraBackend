using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    /// <summary>
    /// Controller quản lý SubscriptionAccount - kế thừa BaseController để tận dụng CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionAccountController : BaseController<ISubscriptionAccountService, SubscriptionAccountDto>
    {
        private readonly ISubscriptionAccountService _subscriptionAccountService;

        public SubscriptionAccountController(ISubscriptionAccountService subscriptionAccountService) : base(subscriptionAccountService)
        {
            _subscriptionAccountService = subscriptionAccountService;
        }

        /// <summary>
        /// Lấy danh sách tất cả đăng ký gói
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var subscriptionAccounts = await _subscriptionAccountService.GetAllAsync();
                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách đăng ký gói thành công",
                    data = subscriptionAccounts
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
        /// Lấy thông tin đăng ký gói theo ID - Override từ BaseController
        /// </summary>
        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var subscriptionAccount = await _subscriptionAccountService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Lấy thông tin đăng ký gói thành công",
                    data = subscriptionAccount
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
        /// Tạo đăng ký gói mới - Sử dụng CreateSubscriptionAccountDto
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionAccountDto dto)
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

                var subscriptionAccount = await _subscriptionAccountService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = subscriptionAccount.Id }, new
                {
                    success = true,
                    message = "Tạo đăng ký gói thành công",
                    data = subscriptionAccount
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
        /// Cập nhật đăng ký gói - Sử dụng UpdateSubscriptionAccountDto
        /// </summary>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubscriptionAccountDto dto)
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

                var subscriptionAccount = await _subscriptionAccountService.UpdateAsync(id, dto);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật đăng ký gói thành công",
                    data = subscriptionAccount
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
        /// Xóa đăng ký gói - Override từ BaseController
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _subscriptionAccountService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Xóa đăng ký gói thành công"
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
