using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class PrescriptionService : BaseService<Prescription, PrescriptionDto>, IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.PrescriptionRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllAsync()
        {
            var prescriptions = await _unitOfWork.PrescriptionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<PrescriptionDto> GetByIdAsync(Guid id)
        {
            var prescription = await _unitOfWork.PrescriptionRepository.GetByIdAsync(id);
            if (prescription == null)
                throw new NotFoundException($"Không tìm thấy đơn thuốc với ID: {id}");
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto dto)
        {
            var prescription = _mapper.Map<Prescription>(dto);
            await _unitOfWork.PrescriptionRepository.AddAsync(prescription);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> UpdateAsync(Guid id, UpdatePrescriptionDto dto)
        {
            var prescription = await _unitOfWork.PrescriptionRepository.GetByIdAsync(id);
            if (prescription == null)
                throw new NotFoundException($"Không tìm thấy đơn thuốc với ID: {id}");
            _mapper.Map(dto, prescription);
            prescription.SetUpdatedDate();
            await _unitOfWork.PrescriptionRepository.UpdateAsync(prescription);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var prescription = await _unitOfWork.PrescriptionRepository.GetByIdAsync(id);
            if (prescription == null)
                throw new NotFoundException($"Không tìm thấy đơn thuốc với ID: {id}");
            await _unitOfWork.PrescriptionRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
