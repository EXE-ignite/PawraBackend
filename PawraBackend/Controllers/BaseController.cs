#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PawraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TService, TDto> : ControllerBase
    {
        protected readonly TService _service;

        public BaseController(TService service)
        {
            _service = service;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto dto)
        {
            try
            {
                var method = typeof(TService).GetMethod("Create");
                if (method == null)
                    return BadRequest(new { success = false, message = "Create method not implemented in service." });

                var invokeResult = method.Invoke(_service, new object[] { dto });
                if (invokeResult == null)
                    return StatusCode(500, new { success = false, message = "Method invocation failed." });

                var result = await ((Task<TDto>)invokeResult)!;
                return Created(string.Empty, new { success = true, message = "Tạo mới thành công", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public virtual async Task<IActionResult> Get([FromQuery] int pageSize = 100, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var method = typeof(TService).GetMethod("Read", new[] { typeof(int), typeof(int) });
                if (method == null)
                    return BadRequest(new { success = false, message = "Read method not implemented in service." });

                var invokeResult = method.Invoke(_service, new object[] { pageSize, pageNumber });
                if (invokeResult == null)
                    return StatusCode(500, new { success = false, message = "Method invocation failed." });

                var result = await ((Task<List<TDto>>)invokeResult)!;
                return Ok(new { success = true, message = "Lấy danh sách thành công", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var method = typeof(TService).GetMethod("Read", new[] { typeof(Guid) });
                if (method == null)
                    return BadRequest(new { success = false, message = "Read by Id method not implemented in service." });

                var invokeResult = method.Invoke(_service, new object[] { id });
                if (invokeResult == null)
                    return StatusCode(500, new { success = false, message = "Method invocation failed." });

                var result = await ((Task<TDto>)invokeResult)!;
                return Ok(new { success = true, message = "Lấy thông tin thành công", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TDto dto)
        {
            try
            {
                var method = typeof(TService).GetMethod("Update");
                if (method == null)
                    return BadRequest(new { success = false, message = "Update method not implemented in service." });

                var invokeResult = method.Invoke(_service, new object[] { dto });
                if (invokeResult == null)
                    return StatusCode(500, new { success = false, message = "Method invocation failed." });

                await ((Task)invokeResult)!;
                return Ok(new { success = true, message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var method = typeof(TService).GetMethod("Delete");
                if (method == null)
                    return BadRequest(new { success = false, message = "Delete method not implemented in service." });

                var invokeResult = method.Invoke(_service, new object[] { id });
                if (invokeResult == null)
                    return StatusCode(500, new { success = false, message = "Method invocation failed." });

                await ((Task)invokeResult)!;
                return Ok(new { success = true, message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }
}
