using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class ClinicService : BaseService<Clinic, ClinicDto>, IClinicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.ClinicRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClinicDto>> GetAllAsync()
        {
            var clinics = await _unitOfWork.ClinicRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClinicDto>>(clinics);
        }

        public async Task<ClinicDto> GetByIdAsync(Guid id)
        {
            var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id);

            if (clinic == null)
            {
                throw new NotFoundException($"Không tìm thấy phòng khám với ID: {id}");
            }

            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<ClinicDto> CreateAsync(CreateClinicDto dto)
        {
            var clinic = _mapper.Map<Clinic>(dto);
            await _unitOfWork.ClinicRepository.AddAsync(clinic);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<ClinicDto> UpdateAsync(Guid id, UpdateClinicDto dto)
        {
            var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id);

            if (clinic == null)
            {
                throw new NotFoundException($"Không tìm thấy phòng khám với ID: {id}");
            }

            _mapper.Map(dto, clinic);
            clinic.SetUpdatedDate();

            await _unitOfWork.ClinicRepository.UpdateAsync(clinic);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id);

            if (clinic == null)
            {
                throw new NotFoundException($"Không tìm thấy phòng khám với ID: {id}");
            }

            await _unitOfWork.ClinicRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
