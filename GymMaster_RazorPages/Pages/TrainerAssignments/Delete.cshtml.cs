using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.TrainerAssignments
{
    public class DeleteModel : PageModel
    {
        private readonly ITrainerAssignmentService _trainerAssignmentService;

        public DeleteModel(ITrainerAssignmentService trainerAssignmentService)
        {
            _trainerAssignmentService = trainerAssignmentService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deleted = await _trainerAssignmentService.DeleteAsync(id.Value);
            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
