using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class ClinicVaccineService : BaseService<ClinicVaccine, ClinicVaccineDto>, IClinicVaccineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicVaccineService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.ClinicVaccineRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClinicVaccineDto>> GetAllAsync()
        {
            var clinicVaccines = await _unitOfWork.ClinicVaccineRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClinicVaccineDto>>(clinicVaccines);
        }

        public async Task<ClinicVaccineDto> GetByIdAsync(Guid id)
        {
            var clinicVaccine = await _unitOfWork.ClinicVaccineRepository.GetByIdAsync(id);
            if (clinicVaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine phòng khám với ID: {id}");
            return _mapper.Map<ClinicVaccineDto>(clinicVaccine);
        }

        public async Task<ClinicVaccineDto> CreateAsync(CreateClinicVaccineDto dto)
        {
            var clinicVaccine = _mapper.Map<ClinicVaccine>(dto);
            await _unitOfWork.ClinicVaccineRepository.AddAsync(clinicVaccine);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClinicVaccineDto>(clinicVaccine);
        }

        public async Task<ClinicVaccineDto> UpdateAsync(Guid id, UpdateClinicVaccineDto dto)
        {
            var clinicVaccine = await _unitOfWork.ClinicVaccineRepository.GetByIdAsync(id);
            if (clinicVaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine phòng khám với ID: {id}");
            _mapper.Map(dto, clinicVaccine);
            clinicVaccine.SetUpdatedDate();
            await _unitOfWork.ClinicVaccineRepository.UpdateAsync(clinicVaccine);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClinicVaccineDto>(clinicVaccine);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var clinicVaccine = await _unitOfWork.ClinicVaccineRepository.GetByIdAsync(id);
            if (clinicVaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine phòng khám với ID: {id}");
            await _unitOfWork.ClinicVaccineRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
