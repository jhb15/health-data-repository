using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthDataRepository.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc", Policy = "Administrator")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "ActivityTypes");
        }
    }
}