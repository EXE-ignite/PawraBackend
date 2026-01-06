using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionAccountController : ControllerBase
    {
        private readonly ISubscriptionAccountService _subscriptionAccountService;

        public SubscriptionAccountController(ISubscriptionAccountService subscriptionAccountService)
        {
            _subscriptionAccountService = subscriptionAccountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var subscriptions = await _subscriptionAccountService.GetAllAsync();
                return Ok(new { success = true, message = "Lấy danh sách đăng ký thành công", data = subscriptions });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var subscription = await _subscriptionAccountService.GetByIdAsync(id);
                return Ok(new { success = true, message = "Lấy thông tin đăng ký thành công", data = subscription });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionAccountDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                }

                var subscription = await _subscriptionAccountService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = subscription.Id }, new { success = true, message = "Tạo đăng ký thành công", data = subscription });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubscriptionAccountDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                }

                var subscription = await _subscriptionAccountService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Cập nhật đăng ký thành công", data = subscription });
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
                await _subscriptionAccountService.DeleteAsync(id);
                return Ok(new { success = true, message = "Xóa đăng ký thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

