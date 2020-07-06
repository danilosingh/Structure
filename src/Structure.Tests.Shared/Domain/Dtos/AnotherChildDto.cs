using Structure.Application;

namespace Structure.Tests.Shared.Domain.Dtos
{
    public  class AnotherChildDto : IEntityDto<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
