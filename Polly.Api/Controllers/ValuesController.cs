using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Polly.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet("retry")]
        public ActionResult<IEnumerable<string>> retry()
        {
            return new string[] { "value1", "value2", "value3" };
        }
    }
}
