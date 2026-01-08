using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class ServiceService : BaseService<Pawra.DAL.Entities.Service, ServiceDto>, IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.ServiceRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllAsync()
        {
            var services = await _unitOfWork.ServiceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ServiceDto>>(services);
        }

        public async Task<ServiceDto> GetByIdAsync(Guid id)
        {
            var service = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
            if (service == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ với ID: {id}");
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
        {
            var service = _mapper.Map<Pawra.DAL.Entities.Service>(dto);
            await _unitOfWork.ServiceRepository.AddAsync(service);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<ServiceDto> UpdateAsync(Guid id, UpdateServiceDto dto)
        {
            var service = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
            if (service == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ với ID: {id}");
            _mapper.Map(dto, service);
            service.SetUpdatedDate();
            await _unitOfWork.ServiceRepository.UpdateAsync(service);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var service = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
            if (service == null)
                throw new NotFoundException($"Không tìm thấy dịch vụ với ID: {id}");
            await _unitOfWork.ServiceRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
