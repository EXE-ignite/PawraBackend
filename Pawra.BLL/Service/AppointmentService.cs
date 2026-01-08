using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class AppointmentService : BaseService<Appointment, AppointmentDto>, IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.AppointmentRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> GetByIdAsync(Guid id)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new NotFoundException($"Không tìm thấy lịch hẹn với ID: {id}");
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            var appointment = _mapper.Map<Appointment>(dto);
            await _unitOfWork.AppointmentRepository.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> UpdateAsync(Guid id, UpdateAppointmentDto dto)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new NotFoundException($"Không tìm thấy lịch hẹn với ID: {id}");
            _mapper.Map(dto, appointment);
            appointment.SetUpdatedDate();
            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new NotFoundException($"Không tìm thấy lịch hẹn với ID: {id}");
            await _unitOfWork.AppointmentRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
