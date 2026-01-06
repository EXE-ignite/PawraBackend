using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class ClinicManagerService : IClinicManagerService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public ClinicManagerService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClinicManagerDto>> GetAllAsync()
        {
            var managers = await _context.ClinicManagers.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<ClinicManagerDto>>(managers);
        }

        public async Task<ClinicManagerDto> GetByIdAsync(Guid id)
        {
            var manager = await _context.ClinicManagers.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null) throw new NotFoundException($"Không tìm thấy quản lý phòng khám với ID: {id}");
            return _mapper.Map<ClinicManagerDto>(manager);
        }

        public async Task<ClinicManagerDto> CreateAsync(CreateClinicManagerDto dto)
        {
            var manager = _mapper.Map<ClinicManager>(dto);
            _context.ClinicManagers.Add(manager);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicManagerDto>(manager);
        }

        public async Task<ClinicManagerDto> UpdateAsync(Guid id, UpdateClinicManagerDto dto)
        {
            var manager = await _context.ClinicManagers.FindAsync(id);
            if (manager == null) throw new NotFoundException($"Không tìm thấy quản lý phòng khám với ID: {id}");
            manager.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<ClinicManagerDto>(manager);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var manager = await _context.ClinicManagers.FindAsync(id);
            if (manager == null) throw new NotFoundException($"Không tìm thấy quản lý phòng khám với ID: {id}");
            _context.ClinicManagers.Remove(manager);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

