using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public PrescriptionService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllAsync()
        {
            var prescriptions = await _context.Prescriptions.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<PrescriptionDto> GetByIdAsync(Guid id)
        {
            var prescription = await _context.Prescriptions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (prescription == null) throw new NotFoundException($"Không tìm thấy đơn thuốc với ID: {id}");
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto dto)
        {
            var prescription = _mapper.Map<Prescription>(dto);
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> UpdateAsync(Guid id, UpdatePrescriptionDto dto)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) throw new NotFoundException($"Không tìm thấy đơn thuốc với ID: {id}");
            _mapper.Map(dto, prescription);
            prescription.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null) throw new NotFoundException($"Không tìm thấy đơn thuốc với ID: {id}");
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

