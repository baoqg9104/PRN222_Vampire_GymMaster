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
using System.Security.Claims;

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

        public TrainerAssignment TrainerAssignment { get; set; } = default!;

        // Bind individual properties only
        [BindProperty]
        public int MemberId { get; set; }

        [BindProperty]
        public int TrainerId { get; set; }

        [BindProperty]
        public int MembershipId { get; set; }

        [BindProperty]
        public DateOnly? StartDate { get; set; }

        [BindProperty]
        public DateOnly? EndDate { get; set; }

        [BindProperty]
        public string? Goals { get; set; }

        // Properties to check user role
        public bool IsMemberRequest { get; set; }
        public bool IsAdmin { get; set; }
        public string CurrentUserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(int? memberId)
        {
            // Get current user info
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUser = await _userService.GetByIdAsync(currentUserId);
            CurrentUserRole = currentUser?.Role ?? "";

            // Check permissions - only Admin and Member can access
            if (CurrentUserRole != "Admin" && CurrentUserRole != "Member")
            {
                return RedirectToPage("/Dashboard/Index");
            }
            if (CurrentUserRole == "Admin") IsAdmin = true;
            if (CurrentUserRole == "Member" || memberId.HasValue) IsMemberRequest = true;

            if (IsMemberRequest && CurrentUserRole == "Member")
            {
                // Member can only request for themselves
                MemberId = currentUserId;
            }
            else if (memberId.HasValue && IsAdmin)
            {
                // Admin creating assignment for specific member
                MemberId = memberId.Value;
            }
            else if (IsAdmin)
            {
                // Admin creating new assignment - will select member
                MemberId = 0;
            }

            TrainerId = 0;
            MembershipId = 0;

            // Set default StartDate for Admin
            if (IsAdmin)
            {
                StartDate = DateOnly.FromDateTime(DateTime.Today);
            }

            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Get current user info
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUser = await _userService.GetByIdAsync(currentUserId);
            CurrentUserRole = currentUser?.Role ?? "";

            // Check permissions
            if (CurrentUserRole != "Admin" && CurrentUserRole != "Member")
            {
                return RedirectToPage("/Dashboard/Index");
            }

            if (CurrentUserRole == "Admin") IsAdmin = true;
            else if (CurrentUserRole == "Member") IsMemberRequest = true;

            // Debug logging
            System.Diagnostics.Debug.WriteLine($"MemberId received: {this.MemberId}");
            System.Diagnostics.Debug.WriteLine($"TrainerId received: {this.TrainerId}");
            System.Diagnostics.Debug.WriteLine($"MembershipId received: {this.MembershipId}");
            System.Diagnostics.Debug.WriteLine($"IsAdmin: {IsAdmin}");
            System.Diagnostics.Debug.WriteLine($"IsMemberRequest: {IsMemberRequest}");

            // Validation
            bool isValid = true;
            var errorMessages = new List<string>();
            if (IsMemberRequest) this.MemberId = currentUserId;

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

            // Member can only request for themselves
            if (IsMemberRequest && MemberId != currentUserId)
            {
                errorMessages.Add("You can only request trainer assignment for yourself.");
                isValid = false;
            }

            // Admin must provide StartDate
            if (IsAdmin && !StartDate.HasValue)
            {
                errorMessages.Add("Start Date is required.");
                isValid = false;
            }

            // Date validation - simple check: Start Date should not be after End Date
            if (this.EndDate.HasValue)
            {
                var effectiveStartDate = IsAdmin && StartDate.HasValue ? StartDate.Value : DateOnly.FromDateTime(DateTime.Today);

                if (this.EndDate.Value <= effectiveStartDate)
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

                await LoadSelectListsAsync();
                return Page();
            }

            // Create TrainerAssignment
            TrainerAssignment = new TrainerAssignment
            {
                MemberId = this.MemberId,
                TrainerId = this.TrainerId,
                MembershipId = this.MembershipId,
                StartDate = IsAdmin ? StartDate.Value : DateOnly.FromDateTime(DateTime.Today), // Admin sets date, Member gets today
                EndDate = this.EndDate,
                Goals = this.Goals,
                IsActive = true
            };

            try
            {
                await _trainerAssignmentService.AddAsync(TrainerAssignment);

                // Redirect based on who created the assignment
                if (IsMemberRequest)
                {
                    TempData["SuccessMessage"] = "Yêu cầu phân công huấn luyện viên đã được gửi thành công!";
                    return RedirectToPage("/Dashboard/MemberDashboard");
                }
                else
                {
                    TempData["SuccessMessage"] = "Trainer assignment created successfully!";
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the trainer assignment: " + ex.Message);
                await LoadSelectListsAsync();
                return Page();
            }
        }

        private async Task LoadSelectListsAsync()
        {
            var users = await _userService.GetAllAsync();

            if (IsMemberRequest && CurrentUserRole == "Member")
            {
                // Member request: only load current member and their current membership
                var currentMember = users.FirstOrDefault(u => u.UserId == MemberId);
                if (currentMember != null)
                {
                    // Create display text with full name if available, otherwise use email
                    var displayText = !string.IsNullOrEmpty(currentMember.FirstName) || !string.IsNullOrEmpty(currentMember.LastName)
                        ? $"{currentMember.FirstName} {currentMember.LastName}".Trim()
                        : currentMember.Email;

                    var memberSelectList = new[]
                    {
                        new { UserId = currentMember.UserId, DisplayText = displayText }
                    };

                    ViewData["MemberId"] = new SelectList(memberSelectList, "UserId", "DisplayText", MemberId);
                }

                // Load current membership for member
                var currentMembership = await _membershipService.GetCurrentMembershipAsync(MemberId);
                if (currentMembership != null)
                {
                    var membershipList = new[]
                    {
                        new
                        {
                            MembershipId = currentMembership.MembershipId,
                            DisplayText = $"{currentMembership.Plan?.Name ?? "Unknown Plan"} ({currentMembership.StartDate:dd/MM/yyyy} - {currentMembership.EndDate:dd/MM/yyyy})"
                        }
                    };
                    ViewData["MembershipId"] = new SelectList(membershipList, "MembershipId", "DisplayText", currentMembership.MembershipId);
                }
                else
                {
                    ViewData["MembershipId"] = new SelectList(new List<object>(), "MembershipId", "DisplayText");
                }
            }
            else if (IsAdmin)
            {
                // Admin: load all members and their current memberships
                var members = users.Where(u => u.Role == "Member").ToList();

                // Create better display text for members
                var memberSelectList = members.Select(m => new
                {
                    UserId = m.UserId,
                    DisplayText = !string.IsNullOrEmpty(m.FirstName) || !string.IsNullOrEmpty(m.LastName)
                        ? $"{m.FirstName} {m.LastName}".Trim() + $" ({m.Email})"
                        : m.Email
                }).ToList();

                ViewData["MemberId"] = new SelectList(memberSelectList, "UserId", "DisplayText", TrainerAssignment?.MemberId);

                // Load current membership for each member
                var allCurrentMemberships = new List<object>();
                foreach (var member in members)
                {
                    var currentMembership = await _membershipService.GetCurrentMembershipAsync(member.UserId);
                    if (currentMembership != null)
                    {
                        allCurrentMemberships.Add(new
                        {
                            MembershipId = currentMembership.MembershipId,
                            DisplayText = $"Member: {member.Email} - Plan: {currentMembership.Plan?.Name ?? "Unknown"} ({currentMembership.StartDate:dd/MM/yyyy} - {currentMembership.EndDate:dd/MM/yyyy})"
                        });
                    }
                }

                ViewData["MembershipId"] = new SelectList(allCurrentMemberships, "MembershipId", "DisplayText", TrainerAssignment?.MembershipId);
            }

            // Load all trainers for both cases
            var trainers = users.Where(u => u.Role == "Trainer").ToList();

            // Create better display text for trainers
            var trainerSelectList = trainers.Select(t => new
            {
                UserId = t.UserId,
                DisplayText = !string.IsNullOrEmpty(t.FirstName) || !string.IsNullOrEmpty(t.LastName)
                    ? $"{t.FirstName} {t.LastName}".Trim() + $" ({t.Email})"
                    : t.Email
            }).ToList();

            ViewData["TrainerId"] = new SelectList(trainerSelectList, "UserId", "DisplayText", TrainerAssignment?.TrainerId);
        }
    }
}