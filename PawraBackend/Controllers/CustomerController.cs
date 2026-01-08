using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawra.BLL.DTOs;
using Pawra.BLL.Interfaces;

namespace PawraBackend.Controllers
{
    /// <summary>
    /// Controller quản lý Customer - kế thừa BaseController để tận dụng CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : BaseController<ICustomerService, CustomerDto>
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService) : base(customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Lấy danh sách tất cả khách hàng
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách khách hàng thành công",
                    data = customers
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
        /// Lấy thông tin khách hàng theo ID - Override từ BaseController
        /// </summary>
        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Lấy thông tin khách hàng thành công",
                    data = customer
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
        /// Tạo khách hàng mới - Sử dụng CreateCustomerDto
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
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

                var customer = await _customerService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = customer.Id }, new
                {
                    success = true,
                    message = "Tạo khách hàng thành công",
                    data = customer
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
        /// Cập nhật khách hàng - Sử dụng UpdateCustomerDto
        /// </summary>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerDto dto)
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

                var customer = await _customerService.UpdateAsync(id, dto);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật khách hàng thành công",
                    data = customer
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
        /// Xóa khách hàng - Override từ BaseController
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Xóa khách hàng thành công"
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
