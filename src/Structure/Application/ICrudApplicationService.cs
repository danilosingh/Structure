using Structure.Collections;
using Structure.Domain.Entities;
using Structure.Domain.Queries;
using System.Threading.Tasks;

namespace Structure.Application
{
    public interface ICrudApplicationService<TEntity, TId, TEntityDto> : ICrudApplicationService<TEntity, TId, FilterableQueryInput, TEntityDto>
      where TEntity : IEntity<TId>
      where TEntityDto : IEntityDto<TId>
    { }

    public interface ICrudApplicationService<TEntity, TId, TQueryInfo, TEntityDto> : ICrudApplicationService<TEntity, TId, TQueryInfo, TEntityDto, TEntityDto>
        where TEntity : IEntity<TId>
        where TEntityDto : IEntityDto<TId>
    { }

    public interface ICrudApplicationService<TEntity, TId, TQueryInfo, TEditingDto, TGetAllDto> : ICrudApplicationService<TEntity, TId, TQueryInfo, TEditingDto, TGetAllDto, TEditingDto>
        where TEntity : IEntity<TId>
        where TGetAllDto : IEntityDto<TId>
        where TEditingDto : IEntityDto<TId>
    { }

    public interface ICrudApplicationService<TEntity, TId, TQueryInfo, TEditingDto, TGetAllDto, TGetDto> : IApplicationService
        where TEntity : IEntity<TId>
        where TEditingDto : IEntityDto<TId>
        where TGetDto : IEntityDto<TId>
        where TGetAllDto : IEntityDto<TId>
    {
        Task<TGetDto> GetAsync(TId id);
        Task<IPagedList<TGetAllDto>> GetAllAsync(TQueryInfo queryInfo);
        Task<TGetDto> CreateAsync(TEditingDto dto);
        Task<TGetDto> UpdateAsync(TEditingDto dto);
        Task DeleteAsync(TId id);
    }
}
