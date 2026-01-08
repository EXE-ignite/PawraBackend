using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class ClinicService : IClinicService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public ClinicService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClinicDto>> GetAllAsync()
        {
            var clinics = await _context.Clinics
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ClinicDto>>(clinics);
        }

        public async Task<ClinicDto> GetByIdAsync(Guid id)
        {
            var clinic = await _context.Clinics
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clinic == null)
            {
                throw new NotFoundException($"Không tìm thấy phòng khám với ID: {id}");
            }

            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<ClinicDto> CreateAsync(CreateClinicDto dto)
        {
            var clinic = _mapper.Map<Clinic>(dto);
            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<ClinicDto> UpdateAsync(Guid id, UpdateClinicDto dto)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic == null)
            {
                throw new NotFoundException($"Không tìm thấy phòng khám với ID: {id}");
            }

            _mapper.Map(dto, clinic);
            clinic.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic == null)
            {
                throw new NotFoundException($"Không tìm thấy phòng khám với ID: {id}");
            }

            _context.Clinics.Remove(clinic);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

