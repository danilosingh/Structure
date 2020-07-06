using Microsoft.Extensions.DependencyInjection;
using Structure.Collections;
using Structure.Domain.Entities;
using Structure.Domain.Queries;
using Structure.Domain.Repositories;
using Structure.Extensions;
using Structure.Validation;
using System.Threading.Tasks;

namespace Structure.Application
{
    public class CrudApplicationService<TEntity, TId, TEntityDto, TRepository> : CrudApplicationService<TEntity, TId, FilterableQueryInput, TEntityDto, TEntityDto, TRepository>
        where TEntity : IEntity<TId>
        where TEntityDto : IEntityDto<TId>
        where TRepository : IRepository<TEntity, TId, FilterableQueryInput>
    {
        public CrudApplicationService(TRepository repository) : base(repository)
        { }
    }


    public class CrudApplicationService<TEntity, TId, TQueryInput, TEntityDto, TRepository> : CrudApplicationService<TEntity, TId, TQueryInput, TEntityDto, TEntityDto, TRepository>
    where TEntity : IEntity<TId>
    where TEntityDto : IEntityDto<TId>
    where TRepository : IRepository<TEntity, TId, TQueryInput>
    {
        public CrudApplicationService(TRepository repository) : base(repository)
        { }
    }


    public class CrudApplicationService<TEntity, TId, TQueryInput, TEditingDto, TGetDto, TRepository> : CrudApplicationService<TEntity, TId, TQueryInput, TEditingDto, TGetDto, TEditingDto, TRepository>
        where TEntity : IEntity<TId>
        where TGetDto : IEntityDto<TId>
        where TEditingDto : IEntityDto<TId>
        where TRepository : IRepository<TEntity, TId, TQueryInput>
    {
        public CrudApplicationService(TRepository repository) : base(repository)
        { }
    }

    public class CrudApplicationService<TEntity, TId, TQueryInput, TEditingDto, TGetAllDto, TGetDto, TRepository> : ApplicationService,
        ICrudApplicationService<TEntity, TId, TQueryInput, TEditingDto, TGetAllDto, TGetDto>
        where TEntity : IEntity<TId>
        where TGetDto : IEntityDto<TId>
        where TGetAllDto : IEntityDto<TId>
        where TEditingDto : IEntityDto<TId>
        where TRepository : IRepository<TEntity, TId, TQueryInput>
    {
        protected readonly TRepository repository;

        public CrudApplicationService(TRepository repository)
        {
            this.repository = repository;
        }

        public virtual async Task<TGetDto> GetAsync(TId id)
        {
            var entity = await repository.GetFullAsync(id);
            return ConvertToDto(entity);
        }

        public virtual async Task<IPagedList<TGetAllDto>> GetAllAsync(TQueryInput queryInput)
        {
            PrepareQueryInput(queryInput);
            return ConvertToDto(await repository.GetAllAsync(queryInput));
        }

        protected virtual TGetDto ConvertToDto(TEntity entity)
        {
            return Adapter.To<TGetDto>(entity);
        }

        protected virtual void PrepareQueryInput(TQueryInput queryInput)
        { }

        protected virtual IPagedList<TGetAllDto> ConvertToDto(IPagedList<TEntity> all)
        {
            return all.As((entity) => Adapter.To<TGetAllDto>(entity));
        }

        public virtual async Task<TGetDto> CreateAsync(TEditingDto dto)
        {
            var entity = ConvertToEntity(dto, default);

            using (var unitOfwork = UowManager.Begin())
            {
                await OnBeforeCreateAsync(dto, entity);
                await OnBeforeCreateOrUpdateAsync(dto, entity);
                
                await repository.CreateAsync(entity);
                
                await OnAfterCreateAsync(dto, entity);
                await OnAfterCreateOrUpdateAsync(dto, entity);

                await unitOfwork.CompleteAsync();

                return ConvertToDto(entity);
            }
        }

        public virtual async Task<TGetDto> UpdateAsync(TEditingDto dto)
        {
            var existingEntity = await repository.GetFullAsync(dto.Id);
            var entity = ConvertToEntity(dto, existingEntity);

            using (var unitOfwork = UowManager.Begin())
            {
                await OnBeforeUpdateAsync(dto, entity);
                await OnBeforeCreateOrUpdateAsync(dto, entity);
                
                await repository.UpdateAsync(entity);
                
                await OnAfterUpdateAsync(dto, entity);
                await OnAfterCreateOrUpdateAsync(dto, entity);

                await unitOfwork.CompleteAsync();

                return ConvertToDto(entity);
            }
        }

        protected virtual Task OnAfterUpdateAsync(TEditingDto dto, TEntity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnAfterCreateOrUpdateAsync(TEditingDto dto, TEntity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnAfterCreateAsync(TEditingDto dto, TEntity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnBeforeCreateAsync(TEditingDto dto, TEntity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnBeforeCreateOrUpdateAsync(TEditingDto dto, TEntity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnBeforeUpdateAsync(TEditingDto dto, TEntity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual TEntity ConvertToEntity(TEditingDto dto, TEntity entity)
        {
            return entity.IsDefault() ? Adapter.To<TEntity>(dto) : Adapter.To(dto, entity);
        }

        protected virtual void Validate<TValitator>(TEntity entity)
            where TValitator : class, IObjectValidator<TEntity>
        {
            Validate<TValitator, TEntity>(entity);
        }

        protected virtual void Validate<TValitator, TObject>(TObject @object)
            where TValitator : class, IObjectValidator<TObject>
        {
            var result = ServiceProvider.GetService<TValitator>().Validate(@object);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

        public virtual async Task DeleteAsync(TId id)
        {
            var entity = repository.Get(id);
            await OnBeforeDeleteAsync(id, entity);
            await repository.DeleteAsync(entity);
        }

        protected virtual Task OnBeforeDeleteAsync(TId id, TEntity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("O registro não existe");
            }

            return Task.CompletedTask;
        }
    }
}
