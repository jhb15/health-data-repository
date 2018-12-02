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

        // GET: ActivityMappings
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActivityMapping.ToListAsync());
        }

        // GET: ActivityMappings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityMapping = await _context.ActivityMapping
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityMapping == null)
            {
                return NotFound();
            }

            return View(activityMapping);
        }

        // GET: ActivityMappings/Create
        public IActionResult Create()
        {
            return View();
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
                return RedirectToAction(nameof(Index));
            }
            return View(activityMapping);
        }

        // GET: ActivityMappings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityMapping = await _context.ActivityMapping.FindAsync(id);
            if (activityMapping == null)
            {
                return NotFound();
            }
            return View(activityMapping);
        }

        // POST: ActivityMappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Source,MappingKey")] ActivityMapping activityMapping)
        {
            if (id != activityMapping.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityMapping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityMappingExists(activityMapping.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activityMapping);
        }

        // GET: ActivityMappings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityMapping = await _context.ActivityMapping
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityMapping == null)
            {
                return NotFound();
            }

            return View(activityMapping);
        }

        // POST: ActivityMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityMapping = await _context.ActivityMapping.FindAsync(id);
            _context.ActivityMapping.Remove(activityMapping);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityMappingExists(int id)
        {
            return _context.ActivityMapping.Any(e => e.Id == id);
        }
    }
}
