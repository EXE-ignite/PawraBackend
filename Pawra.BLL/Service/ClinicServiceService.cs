using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class ClinicServiceService : BaseService<Pawra.DAL.Entities.ClinicService, ClinicServiceDto>, IClinicServiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicServiceService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.ClinicServiceRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClinicServiceDto>> GetAllAsync()
        {
            var clinicServices = await _unitOfWork.ClinicServiceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClinicServiceDto>>(clinicServices);
        }

        public async Task<ClinicServiceDto> GetByIdAsync(Guid id)
        {
            var clinicService = await _unitOfWork.ClinicServiceRepository.GetByIdAsync(id);
            if (clinicService == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ phòng khám với ID: {id}");
            return _mapper.Map<ClinicServiceDto>(clinicService);
        }

        public async Task<ClinicServiceDto> CreateAsync(CreateClinicServiceDto dto)
        {
            var clinicService = _mapper.Map<Pawra.DAL.Entities.ClinicService>(dto);
            await _unitOfWork.ClinicServiceRepository.AddAsync(clinicService);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClinicServiceDto>(clinicService);
        }

        public async Task<ClinicServiceDto> UpdateAsync(Guid id, UpdateClinicServiceDto dto)
        {
            var clinicService = await _unitOfWork.ClinicServiceRepository.GetByIdAsync(id);
            if (clinicService == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ phòng khám với ID: {id}");
            _mapper.Map(dto, clinicService);
            clinicService.SetUpdatedDate();
            await _unitOfWork.ClinicServiceRepository.UpdateAsync(clinicService);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClinicServiceDto>(clinicService);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var clinicService = await _unitOfWork.ClinicServiceRepository.GetByIdAsync(id);
            if (clinicService == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ phòng khám với ID: {id}");
            await _unitOfWork.ClinicServiceRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
