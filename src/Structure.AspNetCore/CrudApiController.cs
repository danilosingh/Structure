using Microsoft.AspNetCore.Mvc;
using Structure.Application;
using Structure.Domain.Entities;
using Structure.Domain.Queries;
using System.Threading.Tasks;

namespace Structure.AspNetCore
{
    public class CrudApiController<TEntity, TId, TEntityDto, TAppService> : CrudApiController<TEntity, TId, FilterableQueryInput, TEntityDto, TEntityDto, TAppService>
       where TEntity : IEntity<TId>
       where TEntityDto : IEntityDto<TId>
       where TAppService : ICrudApplicationService<TEntity, TId, FilterableQueryInput, TEntityDto>
    {
        public CrudApiController(TAppService appService) : base(appService)
        {
        }
    }

    public class CrudApiController<TEntity, TId, TQueryInput, TEntityDto, TAppService> :
        CrudApiController<TEntity, TId, TQueryInput, TEntityDto, TEntityDto, TAppService>
        where TEntity : IEntity<TId>
        where TEntityDto : IEntityDto<TId>
        where TAppService : ICrudApplicationService<TEntity, TId, TQueryInput, TEntityDto>
    {
        public CrudApiController(TAppService appService) : base(appService)
        {
        }
    }

    public class CrudApiController<TEntity, TId, TQueryInput, TEditingDto, TGetAllDto, TAppService> : CrudApiController<TEntity, TId, TQueryInput, TEditingDto, TGetAllDto, TEditingDto, TAppService>
       where TEntity : IEntity<TId>
       where TGetAllDto : IEntityDto<TId>
       where TEditingDto : IEntityDto<TId>
       where TAppService : ICrudApplicationService<TEntity, TId, TQueryInput, TEditingDto, TGetAllDto>
    {
        public CrudApiController(TAppService appService) : base(appService)
        {
        }
    }

    public class CrudApiController<TEntity, TId, TQueryInput, TEditingDto, TGetAllDto, TGetDto, TAppService> : ApiControllerBase
         where TEntity : IEntity<TId>
         where TGetDto : IEntityDto<TId>
         where TGetAllDto : IEntityDto<TId>
         where TEditingDto : IEntityDto<TId>
         where TAppService : ICrudApplicationService<TEntity, TId, TQueryInput, TEditingDto, TGetAllDto, TGetDto>
    {
        protected TAppService appService;

        public CrudApiController(TAppService appService)
        {
            this.appService = appService;
        }

        public virtual async Task<IActionResult> GetAsync(TId id)
        {
            return Result(await appService.GetAsync(id), true);
        }

        public virtual async Task<IActionResult> GetAllAsync(TQueryInput input)
        {
            var result = await appService.GetAllAsync(input);
            return Result(result, true);
        }

        [UnitOfWork(Enabled = false)]
        public virtual async Task<IActionResult> Create(TEditingDto dto)
        {
            return Result(await appService.CreateAsync(dto));
        }

        [UnitOfWork(Enabled = false)]
        public virtual async Task<IActionResult> Update(TId id, [FromBody]TEditingDto dto)
        {
            dto.Id = id;
            return Result(await appService.UpdateAsync(dto));
        }

        public virtual async Task<IActionResult> Delete(TId id)
        {
            await appService.DeleteAsync(id);
            return Result(true);
        }
    }
}
