using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.TrainerAssignments
{
    public class CreateModel : PageModel
    {
        private readonly ITrainerAssignmentService _trainerAssignmentService;
        private readonly IUserService _userService;
        private readonly IUserMembershipService _membershipService;

        public CreateModel(
            ITrainerAssignmentService trainerAssignmentService,
            IUserService userService,
            IUserMembershipService membershipService)
        {
            _trainerAssignmentService = trainerAssignmentService;
            _userService = userService;
            _membershipService = membershipService;
        }

        // Remove this - don't bind TrainerAssignment directly
        // [BindProperty]
        public TrainerAssignment TrainerAssignment { get; set; } = default!;

        // Bind individual properties only - no complex objects
        [BindProperty]
        public int MemberId { get; set; }

        [BindProperty]
        public int TrainerId { get; set; }

        [BindProperty]
        public int MembershipId { get; set; }

        [BindProperty]
        public DateOnly? EndDate { get; set; }

        [BindProperty]
        public string? Goals { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Initialize individual properties to 0
            MemberId = 0;
            TrainerId = 0;
            MembershipId = 0;

            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Debug: Check if values are being received
            System.Diagnostics.Debug.WriteLine($"MemberId received: {this.MemberId}");
            System.Diagnostics.Debug.WriteLine($"TrainerId received: {this.TrainerId}");
            System.Diagnostics.Debug.WriteLine($"MembershipId received: {this.MembershipId}");

            // Simple validation - don't rely on ModelState
            bool isValid = true;
            var errorMessages = new List<string>();

            if (this.MemberId == 0)
            {
                errorMessages.Add("The Member field is required.");
                isValid = false;
            }

            if (this.TrainerId == 0)
            {
                errorMessages.Add("The Trainer field is required.");
                isValid = false;
            }

            if (this.MembershipId == 0)
            {
                errorMessages.Add("The Membership field is required.");
                isValid = false;
            }

            if (!isValid)
            {
                // Add errors to ModelState for display
                foreach (var error in errorMessages)
                {
                    ModelState.AddModelError("", error);
                }

                await LoadSelectListsAsync();
                return Page();
            }

            // Create TrainerAssignment from individual properties
            TrainerAssignment = new TrainerAssignment
            {
                MemberId = this.MemberId,
                TrainerId = this.TrainerId,
                MembershipId = this.MembershipId,
                EndDate = this.EndDate,
                Goals = this.Goals,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                IsActive = true
            };

            // Debug ModelState errors (remove this in production)
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState)
                {
                    var key = modelError.Key;
                    var errors = modelError.Value.Errors;
                    foreach (var error in errors)
                    {
                        // Log the error for debugging
                        System.Diagnostics.Debug.WriteLine($"ModelState Error - Key: {key}, Error: {error.ErrorMessage}");
                    }
                }

                // Reload select lists and return to form
                await LoadSelectListsAsync();
                return Page();
            }

            try
            {
                await _trainerAssignmentService.AddAsync(TrainerAssignment);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Handle any service errors
                ModelState.AddModelError("", "An error occurred while creating the trainer assignment: " + ex.Message);
                await LoadSelectListsAsync();
                return Page();
            }
        }

        private async Task LoadSelectListsAsync()
        {
            var users = await _userService.GetAllAsync();
            var memberships = await _membershipService.GetAllAsync();

            // Filter users by role and create select lists
            var members = users.Where(u => u.Role == "Member").ToList();
            var trainers = users.Where(u => u.Role == "Trainer").ToList();

            ViewData["MemberId"] = new SelectList(members, "UserId", "Email", TrainerAssignment?.MemberId);
            ViewData["TrainerId"] = new SelectList(trainers, "UserId", "Email", TrainerAssignment?.TrainerId);
            ViewData["MembershipId"] = new SelectList(memberships, "MembershipId", "MembershipId", TrainerAssignment?.MembershipId);

            // Optional: Add more descriptive display for memberships if you have more properties
            // ViewData["MembershipId"] = new SelectList(memberships, "MembershipId", "MembershipName", TrainerAssignment?.MembershipId);
        }
    }
}