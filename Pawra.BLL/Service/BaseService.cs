using AutoMapper;
using Pawra.BLL.Interfaces;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Services
{
    public class BaseService<TEntity, TDto> : IService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TDto> Create(TDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Reset Id if exists - Guid is auto-generated, no need to reset
            var entity = _mapper.Map<TEntity>(dto);
            var newEntity = await _repository.Create(entity);
            return _mapper.Map<TDto>(newEntity);
        }

        public virtual async Task<List<TDto>> Read(int pageSize, int pageNumber)
        {
            var entities = await _repository.Read(pageSize: pageSize, pageNumber: pageNumber);
            return _mapper.Map<List<TDto>>(entities);
        }

        public virtual async Task<TDto> Read(Guid id)
        {
            var entity = await _repository.Read(id);
            if (entity == null)
                throw new KeyNotFoundException("Entity not found.");

            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task Update(TDto dto)
        {
            var idProp = typeof(TDto).GetProperty("Id");
            if (idProp == null)
                throw new InvalidOperationException("DTO must have an Id property.");

            Guid id = (Guid)idProp.GetValue(dto)!;
            var existing = await _repository.Read(id);
            if (existing == null)
                throw new KeyNotFoundException("Entity not found.");

            var entity = _mapper.Map<TEntity>(dto);
            await _repository.Update(entity);
        }

        public virtual async Task Delete(Guid id)
        {
            var entity = await _repository.Read(id);
            if (entity == null)
                throw new KeyNotFoundException("Entity not found.");

            await _repository.Delete(id);
        }
    }
}
