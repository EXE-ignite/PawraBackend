using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var paymentMethods = await _paymentMethodService.GetAllAsync();
                return Ok(new { success = true, message = "Lấy danh sách phương thức thanh toán thành công", data = paymentMethods });
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
                var paymentMethod = await _paymentMethodService.GetByIdAsync(id);
                return Ok(new { success = true, message = "Lấy thông tin phương thức thanh toán thành công", data = paymentMethod });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentMethodDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var paymentMethod = await _paymentMethodService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = paymentMethod.Id }, new { success = true, message = "Tạo phương thức thanh toán thành công", data = paymentMethod });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePaymentMethodDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
                var paymentMethod = await _paymentMethodService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Cập nhật phương thức thanh toán thành công", data = paymentMethod });
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
                await _paymentMethodService.DeleteAsync(id);
                return Ok(new { success = true, message = "Xóa phương thức thanh toán thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
