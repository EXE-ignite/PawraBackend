using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class PetService : BaseService<Pet, PetDto>, IPetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PetService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.PetRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PetDto>> GetAllAsync()
        {
            var pets = await _unitOfWork.PetRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }

        public async Task<PetDto> GetByIdAsync(Guid id)
        {
            var pet = await _unitOfWork.PetRepository.GetByIdAsync(id);
            if (pet == null)
                throw new NotFoundException($"Không tìm thấy thú cưng với ID: {id}");
            return _mapper.Map<PetDto>(pet);
        }

        public async Task<PetDto> CreateAsync(CreatePetDto dto)
        {
            var pet = _mapper.Map<Pet>(dto);
            await _unitOfWork.PetRepository.AddAsync(pet);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PetDto>(pet);
        }

        public async Task<PetDto> UpdateAsync(Guid id, UpdatePetDto dto)
        {
            var pet = await _unitOfWork.PetRepository.GetByIdAsync(id);
            if (pet == null)
                throw new NotFoundException($"Không tìm thấy thú cưng với ID: {id}");
            _mapper.Map(dto, pet);
            pet.SetUpdatedDate();
            await _unitOfWork.PetRepository.UpdateAsync(pet);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PetDto>(pet);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var pet = await _unitOfWork.PetRepository.GetByIdAsync(id);
            if (pet == null)
                throw new NotFoundException($"Không tìm thấy thú cưng với ID: {id}");
            await _unitOfWork.PetRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
