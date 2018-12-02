using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthDataRepository.Models;

namespace HealthDataRepository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityTypesController : ControllerBase
    {
        private readonly HealthDataRepositoryContext _context;

        public ActivityTypesController(HealthDataRepositoryContext context)
        {
            _context = context;
        }

        // GET: api/ActivityTypes
        [HttpGet]
        public IEnumerable<ActivityType> GetActivityType()
        {
            return _context.ActivityType;
        }

        // GET: api/ActivityTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivityType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activityType = await _context.ActivityType.FindAsync(id);

            if (activityType == null)
            {
                return NotFound();
            }

            return Ok(activityType);
        }

        private bool ActivityTypeExists(int id)
        {
            return _context.ActivityType.Any(e => e.Id == id);
        }
    }
}