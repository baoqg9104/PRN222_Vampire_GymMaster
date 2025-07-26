using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.TrainerAssignments
{
    public class EditModel : PageModel
    {
        private readonly ITrainerAssignmentService _trainerAssignmentService;
        private readonly IUserService _userService;
        private readonly IUserMembershipService _membershipService;

        public EditModel(
            ITrainerAssignmentService trainerAssignmentService,
            IUserService userService,
            IUserMembershipService membershipService)
        {
            _trainerAssignmentService = trainerAssignmentService;
            _userService = userService;
            _membershipService = membershipService;
        }

        // Don't bind the complex object directly
        public TrainerAssignment TrainerAssignment { get; set; } = default!;

        // Bind individual properties instead
        [BindProperty]
        public int AssignmentId { get; set; }

        [BindProperty]
        public int MemberId { get; set; }

        [BindProperty]
        public int TrainerId { get; set; }

        [BindProperty]
        public int MembershipId { get; set; }

        [BindProperty]
        public DateOnly StartDate { get; set; }

        [BindProperty]
        public DateOnly? EndDate { get; set; }

        [BindProperty]
        public string? Goals { get; set; }

        [BindProperty]
        public bool? IsActive { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerAssignment = await _trainerAssignmentService.GetByIdAsync(id.Value);
            if (trainerAssignment == null)
            {
                return NotFound();
            }

            // Populate both the object for display and individual properties
            TrainerAssignment = trainerAssignment;

            // Set individual properties from the loaded object
            AssignmentId = trainerAssignment.AssignmentId;
            MemberId = trainerAssignment.MemberId;
            TrainerId = trainerAssignment.TrainerId;
            MembershipId = trainerAssignment.MembershipId;
            StartDate = trainerAssignment.StartDate;
            EndDate = trainerAssignment.EndDate;
            Goals = trainerAssignment.Goals;
            IsActive = trainerAssignment.IsActive;

            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Debug: Check if values are being received
            System.Diagnostics.Debug.WriteLine($"AssignmentId: {AssignmentId}");
            System.Diagnostics.Debug.WriteLine($"MemberId: {MemberId}");
            System.Diagnostics.Debug.WriteLine($"TrainerId: {TrainerId}");
            System.Diagnostics.Debug.WriteLine($"MembershipId: {MembershipId}");

            // Manual validation
            if (MemberId == 0 || TrainerId == 0 || MembershipId == 0)
            {
                if (MemberId == 0)
                    ModelState.AddModelError(nameof(MemberId), "The Member field is required.");

                if (TrainerId == 0)
                    ModelState.AddModelError(nameof(TrainerId), "The Trainer field is required.");

                if (MembershipId == 0)
                    ModelState.AddModelError(nameof(MembershipId), "The Membership field is required.");

                // Reload the original assignment for display
                TrainerAssignment = await _trainerAssignmentService.GetByIdAsync(AssignmentId);
                await LoadSelectListsAsync();
                return Page();
            }

            // Create updated TrainerAssignment object
            var updatedAssignment = new TrainerAssignment
            {
                AssignmentId = this.AssignmentId,
                MemberId = this.MemberId,
                TrainerId = this.TrainerId,
                MembershipId = this.MembershipId,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Goals = this.Goals,
                IsActive = this.IsActive
            };

            try
            {
                await _trainerAssignmentService.UpdateAsync(updatedAssignment);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the trainer assignment: " + ex.Message);

                // Reload the original assignment for display
                TrainerAssignment = await _trainerAssignmentService.GetByIdAsync(AssignmentId);
                await LoadSelectListsAsync();
                return Page();
            }
        }

        private async Task LoadSelectListsAsync()
        {
            var users = await _userService.GetAllAsync();
            var memberships = await _membershipService.GetAllAsync();

            // Filter users by role
            var members = users.Where(u => u.Role == "Member").ToList();
            var trainers = users.Where(u => u.Role == "Trainer").ToList();

            ViewData["MemberId"] = new SelectList(members, "UserId", "Email", this.MemberId);
            ViewData["TrainerId"] = new SelectList(trainers, "UserId", "Email", this.TrainerId);
            ViewData["MembershipId"] = new SelectList(memberships, "MembershipId", "MembershipId", this.MembershipId);
        }
    }
}