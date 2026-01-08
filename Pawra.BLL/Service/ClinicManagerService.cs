using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class ClinicManagerService : BaseService<ClinicManager, ClinicManagerDto>, IClinicManagerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicManagerService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.ClinicManagerRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClinicManagerDto>> GetAllAsync()
        {
            var managers = await _unitOfWork.ClinicManagerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClinicManagerDto>>(managers);
        }

        public async Task<ClinicManagerDto> GetByIdAsync(Guid id)
        {
            var manager = await _unitOfWork.ClinicManagerRepository.GetByIdAsync(id);
            if (manager == null)
                throw new NotFoundException($"Không tìm thấy quản lý phòng khám với ID: {id}");
            return _mapper.Map<ClinicManagerDto>(manager);
        }

        public async Task<ClinicManagerDto> CreateAsync(CreateClinicManagerDto dto)
        {
            var manager = _mapper.Map<ClinicManager>(dto);
            await _unitOfWork.ClinicManagerRepository.AddAsync(manager);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClinicManagerDto>(manager);
        }

        public async Task<ClinicManagerDto> UpdateAsync(Guid id, UpdateClinicManagerDto dto)
        {
            var manager = await _unitOfWork.ClinicManagerRepository.GetByIdAsync(id);
            if (manager == null)
                throw new NotFoundException($"Không tìm thấy quản lý phòng khám với ID: {id}");
            _mapper.Map(dto, manager);
            manager.SetUpdatedDate();
            await _unitOfWork.ClinicManagerRepository.UpdateAsync(manager);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClinicManagerDto>(manager);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var manager = await _unitOfWork.ClinicManagerRepository.GetByIdAsync(id);
            if (manager == null)
                throw new NotFoundException($"Không tìm thấy quản lý phòng khám với ID: {id}");
            await _unitOfWork.ClinicManagerRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
