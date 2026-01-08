using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class VaccineService : BaseService<Vaccine, VaccineDto>, IVaccineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VaccineService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.VaccineRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VaccineDto>> GetAllAsync()
        {
            var vaccines = await _unitOfWork.VaccineRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VaccineDto>>(vaccines);
        }

        public async Task<VaccineDto> GetByIdAsync(Guid id)
        {
            var vaccine = await _unitOfWork.VaccineRepository.GetByIdAsync(id);
            if (vaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine với ID: {id}");
            return _mapper.Map<VaccineDto>(vaccine);
        }

        public async Task<VaccineDto> CreateAsync(CreateVaccineDto dto)
        {
            var vaccine = _mapper.Map<Vaccine>(dto);
            await _unitOfWork.VaccineRepository.AddAsync(vaccine);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VaccineDto>(vaccine);
        }

        public async Task<VaccineDto> UpdateAsync(Guid id, UpdateVaccineDto dto)
        {
            var vaccine = await _unitOfWork.VaccineRepository.GetByIdAsync(id);
            if (vaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine với ID: {id}");
            _mapper.Map(dto, vaccine);
            vaccine.SetUpdatedDate();
            await _unitOfWork.VaccineRepository.UpdateAsync(vaccine);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VaccineDto>(vaccine);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var vaccine = await _unitOfWork.VaccineRepository.GetByIdAsync(id);
            if (vaccine == null)
                throw new NotFoundException($"Không tìm thấy vaccine với ID: {id}");
            await _unitOfWork.VaccineRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
