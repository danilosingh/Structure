using Microsoft.AspNetCore.Mvc;
using Structure.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.AspNetCoreDemo.Controllers
{
    public class NormativeBasisController : ApiControllerBase
    {
        public IActionResult GetRoles(int id)
        {
            return Ok(new string[] { "Admin" });
        }

       
    }
}
