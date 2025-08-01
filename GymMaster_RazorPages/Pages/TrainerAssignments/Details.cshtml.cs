﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.TrainerAssignments
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ITrainerAssignmentService _trainerAssignmentService;

        public DetailsModel(ITrainerAssignmentService trainerAssignmentService)
        {
            _trainerAssignmentService = trainerAssignmentService;
        }

        public TrainerAssignment TrainerAssignment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerAssignment = await _trainerAssignmentService.GetByIdAsync(id.Value);

            if (trainerAssignment is not null)
            {
                TrainerAssignment = trainerAssignment;
                return Page();
            }

            return NotFound();
        }
    }
}
