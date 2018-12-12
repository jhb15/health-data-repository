﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthDataRepository.Models;
using Microsoft.AspNetCore.Authorization;
using HealthDataRepository.Repositories;

namespace HealthDataRepository.Controllers
{
    [Authorize(AuthenticationSchemes = "oidc", Policy = "Administrator")]
    public class ActivitiesManagementController : Controller
    {
        private readonly IActivityRepository activityRepository;
        private readonly IActivityTypeRepository activityTypeRepository;

        public ActivitiesManagementController(IActivityRepository activityRepository, IActivityTypeRepository activityTypeRepository)
        {
            this.activityRepository = activityRepository;
            this.activityTypeRepository = activityTypeRepository;
        }

        // GET: ActivitiesManagement
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int page = (pageNumber ?? 1);
            return View(await activityRepository.GetAllPaginatedAsync(page, 10));
        }

        // GET: ActivitiesManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await activityRepository.GetByIdAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeId"] = new SelectList(await activityTypeRepository.GetAllAsync(), "Id", "Name", activity.ActivityTypeId);
            return View(activity);
        }

        // POST: ActivitiesManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,StartTimestamp,EndTimestamp,Source,ActivityTypeId,CaloriesBurnt,AverageHeartRate,StepsTaken,MetresTravelled,MetresElevationGained")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await activityRepository.UpdateAsync(activity);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityTypeId"] = new SelectList(await activityTypeRepository.GetAllAsync(), "Id", "Name", activity.ActivityTypeId);
            return View(activity);
        }

        // GET: ActivitiesManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await activityRepository.GetByIdAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: ActivitiesManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await activityRepository.GetByIdAsync(id);
            await activityRepository.DeleteAsync(activity);
            return RedirectToAction(nameof(Index));
        }
    }
}
