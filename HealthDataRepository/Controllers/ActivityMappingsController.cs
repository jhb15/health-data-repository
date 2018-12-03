using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthDataRepository.Models;

namespace HealthDataRepository.Controllers
{
    public class ActivityMappingsController : Controller
    {
        private readonly HealthDataRepositoryContext _context;

        public ActivityMappingsController(HealthDataRepositoryContext context)
        {
            _context = context;
        }

        // POST: ActivityMappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Source,MappingKey,ActivityTypeId")] ActivityMapping activityMapping)
        {
            if (ModelState.IsValid)
            {

                activityMapping.Source = Enum.GetName(typeof(DataSource), Convert.ToInt32(activityMapping.Source));
                _context.Add(activityMapping);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "ActivityTypes", new { Id = activityMapping.ActivityTypeId });
            }
            return View(activityMapping);
        }
    }
}
