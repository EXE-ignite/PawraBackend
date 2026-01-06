using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class PetService : IPetService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public PetService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PetDto>> GetAllAsync()
        {
            var pets = await _context.Pets
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }

        public async Task<PetDto> GetByIdAsync(Guid id)
        {
            var pet = await _context.Pets
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
            {
                throw new NotFoundException($"Không tìm thấy thú cưng với ID: {id}");
            }

            return _mapper.Map<PetDto>(pet);
        }

        public async Task<PetDto> CreateAsync(CreatePetDto dto)
        {
            var pet = _mapper.Map<Pet>(dto);

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            return _mapper.Map<PetDto>(pet);
        }

        public async Task<PetDto> UpdateAsync(Guid id, UpdatePetDto dto)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                throw new NotFoundException($"Không tìm thấy thú cưng với ID: {id}");
            }

            _mapper.Map(dto, pet);
            pet.SetUpdatedDate();

            await _context.SaveChangesAsync();

            return _mapper.Map<PetDto>(pet);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                throw new NotFoundException($"Không tìm thấy thú cưng với ID: {id}");
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

