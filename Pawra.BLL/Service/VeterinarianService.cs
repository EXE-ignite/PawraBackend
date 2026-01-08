using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class VeterinarianService : BaseService<Veterinarian, VeterinarianDto>, IVeterinarianService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VeterinarianService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.VeterinarianRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VeterinarianDto>> GetAllAsync()
        {
            var veterinarians = await _unitOfWork.VeterinarianRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VeterinarianDto>>(veterinarians);
        }

        public async Task<VeterinarianDto> GetByIdAsync(Guid id)
        {
            var veterinarian = await _unitOfWork.VeterinarianRepository.GetByIdAsync(id);
            if (veterinarian == null)
                throw new NotFoundException($"Không tìm thấy bác sĩ thú y với ID: {id}");
            return _mapper.Map<VeterinarianDto>(veterinarian);
        }

        public async Task<VeterinarianDto> CreateAsync(CreateVeterinarianDto dto)
        {
            var veterinarian = _mapper.Map<Veterinarian>(dto);
            await _unitOfWork.VeterinarianRepository.AddAsync(veterinarian);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VeterinarianDto>(veterinarian);
        }

        public async Task<VeterinarianDto> UpdateAsync(Guid id, UpdateVeterinarianDto dto)
        {
            var veterinarian = await _unitOfWork.VeterinarianRepository.GetByIdAsync(id);
            if (veterinarian == null)
                throw new NotFoundException($"Không tìm thấy bác sĩ thú y với ID: {id}");
            _mapper.Map(dto, veterinarian);
            veterinarian.SetUpdatedDate();
            await _unitOfWork.VeterinarianRepository.UpdateAsync(veterinarian);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VeterinarianDto>(veterinarian);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var veterinarian = await _unitOfWork.VeterinarianRepository.GetByIdAsync(id);
            if (veterinarian == null)
                throw new NotFoundException($"Không tìm thấy bác sĩ thú y với ID: {id}");
            await _unitOfWork.VeterinarianRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
