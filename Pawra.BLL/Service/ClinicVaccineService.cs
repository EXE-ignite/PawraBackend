using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class ClinicVaccineService : IClinicVaccineService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public ClinicVaccineService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClinicVaccineDto>> GetAllAsync()
        {
            var clinicVaccines = await _context.ClinicVaccines
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<ClinicVaccineDto>>(clinicVaccines);
        }

        public async Task<ClinicVaccineDto> GetByIdAsync(Guid id)
        {
            var clinicVaccine = await _context.ClinicVaccines
                .AsNoTracking()
                .FirstOrDefaultAsync(cv => cv.Id == id);
            if (clinicVaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine phòng khám với ID: {id}");
            return _mapper.Map<ClinicVaccineDto>(clinicVaccine);
        }

        public async Task<ClinicVaccineDto> CreateAsync(CreateClinicVaccineDto dto)
        {
            var clinicVaccine = _mapper.Map<ClinicVaccine>(dto);
            _context.ClinicVaccines.Add(clinicVaccine);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicVaccineDto>(clinicVaccine);
        }

        public async Task<ClinicVaccineDto> UpdateAsync(Guid id, UpdateClinicVaccineDto dto)
        {
            var clinicVaccine = await _context.ClinicVaccines.FindAsync(id);
            if (clinicVaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine phòng khám với ID: {id}");
            _mapper.Map(dto, clinicVaccine);
            clinicVaccine.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicVaccineDto>(clinicVaccine);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var clinicVaccine = await _context.ClinicVaccines.FindAsync(id);
            if (clinicVaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine phòng khám với ID: {id}");
            _context.ClinicVaccines.Remove(clinicVaccine);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

