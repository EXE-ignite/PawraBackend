using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(Guid id);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task<AppointmentDto> UpdateAsync(Guid id, UpdateAppointmentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
