using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthDataRepository.Models;
using Microsoft.AspNetCore.Authorization;

namespace HealthDataRepository.Controllers
{
    [Route("api/activity")]
    [Authorize(AuthenticationSchemes = "token")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly HealthDataRepositoryContext _context;

        public ActivitiesController(HealthDataRepositoryContext context)
        {
            _context = context;
        }

        // GET: api/activity/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _context.Activity.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        // PUT: api/activity/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity([FromRoute] int id, [FromBody] Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activity.Id)
            {
                return BadRequest();
            }

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/activity
        [HttpPost]
        public async Task<IActionResult> PostActivity([FromBody] Activity activity)
        {
            var activityType = await _context.ActivityType.FindAsync(activity.ActivityTypeId);
            if (activityType == null)
            {
                ModelState.AddModelError("ActivityTypeId", "Invalid ID specified.");
            }

            if (activity.EndTimestamp.CompareTo(activity.StartTimestamp) < 0)
            {
                ModelState.AddModelError("EndTimestamp", "Activity must end after it started.");
            }

            if (!Enum.IsDefined(typeof(DataSource), activity.Source))
            {
                ModelState.AddModelError("Source", $"Must be one of [{String.Join(", ", Enum.GetNames(typeof(DataSource)))}]");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Activity.Add(activity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }

        // DELETE: api/activity/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok(activity);
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}