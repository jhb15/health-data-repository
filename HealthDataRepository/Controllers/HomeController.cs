﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthDataRepository.Controllers
{
    
    public class HomeController : Controller
    {
        [Authorize(AuthenticationSchemes = "oidc")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "ActivityTypes");
        }
    }
}