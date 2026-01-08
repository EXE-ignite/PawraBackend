using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class VaccinationRecordService : BaseService<VaccinationRecord, VaccinationRecordDto>, IVaccinationRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VaccinationRecordService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.VaccinationRecordRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VaccinationRecordDto>> GetAllAsync()
        {
            var records = await _unitOfWork.VaccinationRecordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VaccinationRecordDto>>(records);
        }

        public async Task<VaccinationRecordDto> GetByIdAsync(Guid id)
        {
            var record = await _unitOfWork.VaccinationRecordRepository.GetByIdAsync(id);
            if (record == null)
                throw new NotFoundException($"Không tìm thấy lịch sử tiêm chủng với ID: {id}");
            return _mapper.Map<VaccinationRecordDto>(record);
        }

        public async Task<VaccinationRecordDto> CreateAsync(CreateVaccinationRecordDto dto)
        {
            var record = _mapper.Map<VaccinationRecord>(dto);
            await _unitOfWork.VaccinationRecordRepository.AddAsync(record);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VaccinationRecordDto>(record);
        }

        public async Task<VaccinationRecordDto> UpdateAsync(Guid id, UpdateVaccinationRecordDto dto)
        {
            var record = await _unitOfWork.VaccinationRecordRepository.GetByIdAsync(id);
            if (record == null)
                throw new NotFoundException($"Không tìm thấy lịch sử tiêm chủng với ID: {id}");
            _mapper.Map(dto, record);
            record.SetUpdatedDate();
            await _unitOfWork.VaccinationRecordRepository.UpdateAsync(record);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VaccinationRecordDto>(record);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var record = await _unitOfWork.VaccinationRecordRepository.GetByIdAsync(id);
            if (record == null)
                throw new NotFoundException($"Không tìm thấy lịch sử tiêm chủng với ID: {id}");
            await _unitOfWork.VaccinationRecordRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
