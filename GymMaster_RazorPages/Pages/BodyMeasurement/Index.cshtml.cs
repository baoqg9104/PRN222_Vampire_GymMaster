using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Services;

namespace GymMaster_RazorPages.Pages.BodyMeasurement;

public class IndexModel : PageModel
{
    private readonly IBodyMeasurementService _bodyMeasurementService;
    private readonly ITrainerAssignmentService _trainerAssignmentService;

    public IndexModel(IBodyMeasurementService bodyMeasurementService, ITrainerAssignmentService trainerAssignmentService)
    {
        _bodyMeasurementService = bodyMeasurementService;
        _trainerAssignmentService = trainerAssignmentService;
    }

    public IList<MSSQLServer.EntitiesModels.BodyMeasurement> Measurements { get; set; } = default!;
    public IList<MSSQLServer.EntitiesModels.TrainerAssignment> TrainerAssignments { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (User.IsInRole("Member"))
        {
            var measurements = await _bodyMeasurementService.GetByUserIdAsync(userId);
            Measurements = measurements.OrderByDescending(m => m.MeasuredAt).ToList();
        }
        else if (User.IsInRole("Trainer"))
        {
            var trainerAssignments = await _trainerAssignmentService.GetByTrainerIdAsync(userId);
            TrainerAssignments = trainerAssignments.ToList();
        }
    }
}
