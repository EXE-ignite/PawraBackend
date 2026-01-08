using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class VeterinarianService : IVeterinarianService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public VeterinarianService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VeterinarianDto>> GetAllAsync()
        {
            var veterinarians = await _context.Veterinarians
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<VeterinarianDto>>(veterinarians);
        }

        public async Task<VeterinarianDto> GetByIdAsync(Guid id)
        {
            var veterinarian = await _context.Veterinarians
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
            if (veterinarian == null)
                throw new NotFoundException($"Không tìm thấy bác sĩ thú y với ID: {id}");
            return _mapper.Map<VeterinarianDto>(veterinarian);
        }

        public async Task<VeterinarianDto> CreateAsync(CreateVeterinarianDto dto)
        {
            var veterinarian = _mapper.Map<Veterinarian>(dto);
            _context.Veterinarians.Add(veterinarian);
            await _context.SaveChangesAsync();
            return _mapper.Map<VeterinarianDto>(veterinarian);
        }

        public async Task<VeterinarianDto> UpdateAsync(Guid id, UpdateVeterinarianDto dto)
        {
            var veterinarian = await _context.Veterinarians.FindAsync(id);
            if (veterinarian == null)
                throw new NotFoundException($"Không tìm thấy bác sĩ thú y với ID: {id}");
            _mapper.Map(dto, veterinarian);
            veterinarian.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<VeterinarianDto>(veterinarian);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var veterinarian = await _context.Veterinarians.FindAsync(id);
            if (veterinarian == null)
                throw new NotFoundException($"Không tìm thấy bác sĩ thú y với ID: {id}");
            _context.Veterinarians.Remove(veterinarian);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

