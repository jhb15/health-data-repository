using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthDataRepository.Models;
using Microsoft.AspNetCore.Authorization;
using HealthDataRepository.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HealthDataRepository.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = "token")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityRepository activityRepository;
        private readonly IActivityTypeRepository activityTypeRepository;

        public ActivitiesController(IActivityRepository activityRepository, IActivityTypeRepository activityTypeRepository)
        {
            this.activityRepository = activityRepository;
            this.activityTypeRepository = activityTypeRepository;
        }

        // GET: api/activity/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity([FromRoute] int id)
        {
            var activity = await activityRepository.GetByIdAsync(id);

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
            if (id != activity.Id)
            {
                ModelState.AddModelError("Id", "Id must match URL parameter.");
            }

            await ValidateActivity(activity, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            await activityRepository.UpdateAsync(activity);

            return Ok(activity);
        }

        // POST: api/activity
        [HttpPost]
        public async Task<IActionResult> PostActivity([FromBody] Activity activity)
        {
            await ValidateActivity(activity, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            activity = await activityRepository.AddAsync(activity);

            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }

        // DELETE: api/activity/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity([FromRoute] int id)
        {
            var activity = await activityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            await activityRepository.DeleteAsync(activity);
            return Ok(activity);
        }

        private async Task ValidateActivity(Activity activity, ModelStateDictionary ModelState)
        {
            var activityType = await activityTypeRepository.GetByIdAsync(activity.ActivityTypeId);
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
                ModelState.AddModelError("Source", $"Must be one of {DataSource.Manual.GetValuesAsArrayString()}");
            }
        }
    }
}