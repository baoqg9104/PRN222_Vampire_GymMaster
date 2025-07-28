using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Services;

namespace GymMaster_RazorPages.Pages.BodyMeasurement;

public class CreateModel : PageModel
{
    private readonly IBodyMeasurementService _measurementService;
    private readonly IUserService _userService;
    //private readonly ITrainerAssignmentService _assignmentService;

    public CreateModel(IBodyMeasurementService measurementService, IUserService userService)
    {
        _measurementService = measurementService;
        _userService = userService;
        //_assignmentService = assignmentService;
    }

    [BindProperty]
    public MSSQLServer.EntitiesModels.BodyMeasurement NewMeasurement { get; set; } = new();

    public List<MSSQLServer.EntitiesModels.BodyMeasurement> Measurements { get; set; } = new();
    public string MemberName { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int memberId)
    {
        var trainerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var member = await _userService.GetByIdAsync(memberId);
        if (member == null)
        {
            return NotFound();
        }

        MemberName = $"{member.FirstName} {member.LastName}";
        NewMeasurement.MemberId = memberId;
        Measurements = (await _measurementService.GetByUserIdAsync(memberId)).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("NewMeasurement.Member");

        if (!ModelState.IsValid)
        {
            // Reload data if validation fails
            var member = await _userService.GetByIdAsync(NewMeasurement.MemberId);
            MemberName = $"{member.FirstName} {member.LastName}";
            Measurements = (await _measurementService.GetByUserIdAsync(NewMeasurement.MemberId)).ToList();
            return Page();
        }

        await _measurementService.AddAsync(NewMeasurement);
        TempData["SuccessMessage"] = "Measurement added successfully!";
        return RedirectToPage("./Create", new { memberId = NewMeasurement.MemberId });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var measurement = await _measurementService.GetByIdAsync(id);
        if (measurement == null)
        {
            return NotFound();
        }

        await _measurementService.DeleteAsync(id);
        TempData["SuccessMessage"] = "Measurement deleted successfully!";
        return RedirectToPage("./Create", new { memberId = measurement.MemberId });
    }
}
