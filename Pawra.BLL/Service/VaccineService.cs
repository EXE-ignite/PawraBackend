using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class VaccineService : IVaccineService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public VaccineService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VaccineDto>> GetAllAsync()
        {
            var vaccines = await _context.Vaccines
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<VaccineDto>>(vaccines);
        }

        public async Task<VaccineDto> GetByIdAsync(Guid id)
        {
            var vaccine = await _context.Vaccines
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vaccine == null)
            {
                throw new NotFoundException($"Không tìm thấy vaccine với ID: {id}");
            }

            return _mapper.Map<VaccineDto>(vaccine);
        }

        public async Task<VaccineDto> CreateAsync(CreateVaccineDto dto)
        {
            var vaccine = _mapper.Map<Vaccine>(dto);

            _context.Vaccines.Add(vaccine);
            await _context.SaveChangesAsync();

            return _mapper.Map<VaccineDto>(vaccine);
        }

        public async Task<VaccineDto> UpdateAsync(Guid id, UpdateVaccineDto dto)
        {
            var vaccine = await _context.Vaccines.FindAsync(id);

            if (vaccine == null)
            {
                throw new NotFoundException($"Không tìm thấy vaccine với ID: {id}");
            }

            _mapper.Map(dto, vaccine);
            vaccine.SetUpdatedDate();

            await _context.SaveChangesAsync();

            return _mapper.Map<VaccineDto>(vaccine);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var vaccine = await _context.Vaccines.FindAsync(id);

            if (vaccine == null)
            {
                throw new NotFoundException($"Không tìm thấy vaccine với ID: {id}");
            }

            _context.Vaccines.Remove(vaccine);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

