using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HealthDataRepository.Controllers
{
    public class StatusController : Controller
    {
        [Route("api/status")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
