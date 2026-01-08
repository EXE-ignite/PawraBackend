using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class VaccinationRecordService : IVaccinationRecordService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public VaccinationRecordService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VaccinationRecordDto>> GetAllAsync()
        {
            var records = await _context.VaccinationRecords
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<VaccinationRecordDto>>(records);
        }

        public async Task<VaccinationRecordDto> GetByIdAsync(Guid id)
        {
            var record = await _context.VaccinationRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(vr => vr.Id == id);
            if (record == null)
                throw new NotFoundException($"Không tìm thấy lịch sử tiêm chủng với ID: {id}");
            return _mapper.Map<VaccinationRecordDto>(record);
        }

        public async Task<VaccinationRecordDto> CreateAsync(CreateVaccinationRecordDto dto)
        {
            var record = _mapper.Map<VaccinationRecord>(dto);
            _context.VaccinationRecords.Add(record);
            await _context.SaveChangesAsync();
            return _mapper.Map<VaccinationRecordDto>(record);
        }

        public async Task<VaccinationRecordDto> UpdateAsync(Guid id, UpdateVaccinationRecordDto dto)
        {
            var record = await _context.VaccinationRecords.FindAsync(id);
            if (record == null)
                throw new NotFoundException($"Không tìm thấy lịch sử tiêm chủng với ID: {id}");
            _mapper.Map(dto, record);
            record.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<VaccinationRecordDto>(record);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var record = await _context.VaccinationRecords.FindAsync(id);
            if (record == null)
                throw new NotFoundException($"Không tìm thấy lịch sử tiêm chủng với ID: {id}");
            _context.VaccinationRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

