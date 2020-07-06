using Structure.AutoMapper;
using Structure.Tests.Shared.Domain.Dtos;
using Structure.Tests.Shared.Dtos;
using Structure.Tests.Shared.Entities;
using System.Linq;

namespace Structure.AspNetCoreDemo.Core
{
    public class ApplicationAutoMapperProfile : EntityAutoMapperProfile
    {
        public ApplicationAutoMapperProfile()
        {
            CreateEntityMap<User, UserDto>(configureReverseMap: map => {
                map.ForPath(c => c.CreatorId, map => map.Ignore())
                    .ForPath(c => c.CreationTime, map => map.Ignore())
                    .ForPath(c => c.LastModifierId, map => map.Ignore())
                    .ForPath(c => c.LastModificationTime, map => map.Ignore());
            });
                
            CreateEntityMap<Topic, TopicDto>(configureReverseMap: map =>
            {
                map.AfterMap((c, d) =>
                 {
                     foreach (var item in d.Children)
                     {
                         item.Another = d;
                     }
                 });
            });


            CreateEntityMap<TopicChild, AnotherChildDto>();
            CreateEntityMap<Role, RoleDto>();
        }
    }
}
