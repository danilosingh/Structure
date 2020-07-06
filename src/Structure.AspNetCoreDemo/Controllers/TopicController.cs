using Microsoft.AspNetCore.Mvc;
using Structure.AspNetCore;
using Structure.AspNetCoreDemo.Application;
using Structure.Identity;
using Structure.Tests.Shared.Dtos;
using Structure.Tests.Shared.Entities;
using System;
using System.Collections.Generic;

namespace Structure.AspNetCoreDemo.Controllers
{
    public class TopicController : CrudApiController<Topic, Guid, TopicDto, ITopicAppService>
    {
        private readonly IRoleManager<Role> roleManager;

        public TopicController(ITopicAppService appService, IRoleManager<Role> roleManager) : base(appService)
        {
            this.roleManager = roleManager;
        }
               
        public IActionResult GetPermission()
        {           
            return Ok(roleManager.GetPermissions("1").Result);
        }

        public IList<Topic> GetTopicTree(bool all = true, int? parentTopicId = null)
        {
            if(all)
            { 
            }

            if(parentTopicId == null)
            {

            }
            return new List<Topic>();
        }
    }
}
