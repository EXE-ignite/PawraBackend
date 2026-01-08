using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class ClinicServiceService : IClinicServiceService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public ClinicServiceService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClinicServiceDto>> GetAllAsync()
        {
            var clinicServices = await _context.ClinicServices
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<ClinicServiceDto>>(clinicServices);
        }

        public async Task<ClinicServiceDto> GetByIdAsync(Guid id)
        {
            var clinicService = await _context.ClinicServices
                .AsNoTracking()
                .FirstOrDefaultAsync(cs => cs.Id == id);
            if (clinicService == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ phòng khám với ID: {id}");
            return _mapper.Map<ClinicServiceDto>(clinicService);
        }

        public async Task<ClinicServiceDto> CreateAsync(CreateClinicServiceDto dto)
        {
            var clinicService = _mapper.Map<Pawra.DAL.Entities.ClinicService>(dto);
            _context.ClinicServices.Add(clinicService);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicServiceDto>(clinicService);
        }

        public async Task<ClinicServiceDto> UpdateAsync(Guid id, UpdateClinicServiceDto dto)
        {
            var clinicService = await _context.ClinicServices.FindAsync(id);
            if (clinicService == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ phòng khám với ID: {id}");
            _mapper.Map(dto, clinicService);
            clinicService.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicServiceDto>(clinicService);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var clinicService = await _context.ClinicServices.FindAsync(id);
            if (clinicService == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ phòng khám với ID: {id}");
            _context.ClinicServices.Remove(clinicService);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

