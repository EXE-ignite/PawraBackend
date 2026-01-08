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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var subscriptionAccounts = await _subscriptionAccountService.GetAllAsync();
                return Ok(new { success = true, message = "Lấy danh sách đăng ký gói thành công", data = subscriptionAccounts });
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
                var subscriptionAccount = await _subscriptionAccountService.GetByIdAsync(id);
                return Ok(new { success = true, message = "Lấy thông tin đăng ký gói thành công", data = subscriptionAccount });
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
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var subscriptionAccount = await _subscriptionAccountService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = subscriptionAccount.Id }, new { success = true, message = "Tạo đăng ký gói thành công", data = subscriptionAccount });
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
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var subscriptionAccount = await _subscriptionAccountService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Cập nhật đăng ký gói thành công", data = subscriptionAccount });
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
                return Ok(new { success = true, message = "Xóa đăng ký gói thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
