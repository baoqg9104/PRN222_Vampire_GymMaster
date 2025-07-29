using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GymMaster_RazorPages.Pages.TrainerAssignments
{
    [Authorize(Roles = "Admin")]

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

        public TrainerAssignment TrainerAssignment { get; set; } = default!;

        // Bind individual properties
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
        public bool IsActive { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Check admin permission
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUser = await _userService.GetByIdAsync(currentUserId);

            if (currentUser?.Role != "Admin")
            {
                return RedirectToPage("/Dashboard/Index");
            }

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
            IsActive = (bool)trainerAssignment.IsActive;

            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check admin permission
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUser = await _userService.GetByIdAsync(currentUserId);

            if (currentUser?.Role != "Admin")
            {
                return RedirectToPage("/Dashboard/Index");
            }

            // Debug logging
            System.Diagnostics.Debug.WriteLine($"AssignmentId: {AssignmentId}");
            System.Diagnostics.Debug.WriteLine($"MemberId: {MemberId}");
            System.Diagnostics.Debug.WriteLine($"TrainerId: {TrainerId}");
            System.Diagnostics.Debug.WriteLine($"MembershipId: {MembershipId}");

            // Validation
            bool isValid = true;
            var errorMessages = new List<string>();

            if (MemberId == 0)
            {
                errorMessages.Add("The Member field is required.");
                isValid = false;
            }

            if (TrainerId == 0)
            {
                errorMessages.Add("The Trainer field is required.");
                isValid = false;
            }

            if (MembershipId == 0)
            {
                errorMessages.Add("The Membership field is required.");
                isValid = false;
            }

            // Date validation - simple check: Start Date should not be after End Date
            if (EndDate.HasValue)
            {
                if (EndDate.Value <= StartDate)
                {
                    errorMessages.Add("End Date must be after Start Date.");
                    isValid = false;
                }
            }

            if (!isValid)
            {
                foreach (var error in errorMessages)
                {
                    ModelState.AddModelError("", error);
                }

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
                TempData["SuccessMessage"] = "Trainer assignment updated successfully!";
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

            // Load all members
            var members = users.Where(u => u.Role == "Member").ToList();
            var memberSelectList = members.Select(m => new
            {
                UserId = m.UserId,
                DisplayText = !string.IsNullOrEmpty(m.FirstName) || !string.IsNullOrEmpty(m.LastName)
                    ? $"{m.FirstName} {m.LastName}".Trim() + $" ({m.Email})"
                    : m.Email
            }).ToList();

            ViewData["MemberId"] = new SelectList(memberSelectList, "UserId", "DisplayText", this.MemberId);

            // Load all trainers
            var trainers = users.Where(u => u.Role == "Trainer").ToList();
            var trainerSelectList = trainers.Select(t => new
            {
                UserId = t.UserId,
                DisplayText = !string.IsNullOrEmpty(t.FirstName) || !string.IsNullOrEmpty(t.LastName)
                    ? $"{t.FirstName} {t.LastName}".Trim() + $" ({t.Email})"
                    : t.Email
            }).ToList();

            ViewData["TrainerId"] = new SelectList(trainerSelectList, "UserId", "DisplayText", this.TrainerId);

            // Load all current memberships for all members
            var allCurrentMemberships = new List<object>();
            foreach (var member in members)
            {
                var currentMembership = await _membershipService.GetCurrentMembershipAsync(member.UserId);
                if (currentMembership != null)
                {
                    var memberDisplayName = !string.IsNullOrEmpty(member.FirstName) || !string.IsNullOrEmpty(member.LastName)
                        ? $"{member.FirstName} {member.LastName}".Trim()
                        : member.Email;

                    allCurrentMemberships.Add(new
                    {
                        MembershipId = currentMembership.MembershipId,
                        DisplayText = $"Member: {memberDisplayName} - Plan: {currentMembership.Plan?.Name ?? "Unknown"} ({currentMembership.StartDate:dd/MM/yyyy} - {currentMembership.EndDate:dd/MM/yyyy})"
                    });
                }
            }

            ViewData["MembershipId"] = new SelectList(allCurrentMemberships, "MembershipId", "DisplayText", this.MembershipId);
        }
    }
}