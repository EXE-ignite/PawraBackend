using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class ServiceService : IServiceService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public ServiceService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllAsync()
        {
            var services = await _context.Services
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<ServiceDto>>(services);
        }

        public async Task<ServiceDto> GetByIdAsync(Guid id)
        {
            var service = await _context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ với ID: {id}");
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
        {
            var service = _mapper.Map<Pawra.DAL.Entities.Service>(dto);
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<ServiceDto> UpdateAsync(Guid id, UpdateServiceDto dto)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ với ID: {id}");
            _mapper.Map(dto, service);
            service.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ với ID: {id}");
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

