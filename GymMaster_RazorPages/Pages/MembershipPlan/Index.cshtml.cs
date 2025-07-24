using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MSSQLServer.EntitiesModels;
using Services.Services;

namespace GymMaster_RazorPages.Pages.MembershipPlan
{
    // If needed, apply [Authorize(Roles = "Admin")] or similar
    public class IndexModel : PageModel
    {
        private readonly IMembershipPlanService _membershipPlanService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IMembershipPlanService membershipPlanService, ILogger<IndexModel> logger)
        {
            _membershipPlanService = membershipPlanService;
            _logger = logger;
        }

        public IList<MSSQLServer.EntitiesModels.MembershipPlan> MembershipPlans { get; set; } = new List<MSSQLServer.EntitiesModels.MembershipPlan>();

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CurrentFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageNumber { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? ActiveOnly { get; set; }
        
        public string NameSort { get; set; }
        public string PriceSort { get; set; }
        public string DateSort { get; set; }
        public string DurationSort { get; set; }

        // You can either integrate pagination into the service or continue to use your helper class.
        public PaginatedList<MSSQLServer.EntitiesModels.MembershipPlan> PaginatedPlans { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        
        // Summary statistics
        public int TotalPlans { get; set; }
        public int ActivePlans { get; set; }
        public decimal AveragePrice { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Set up sort parameters
                NameSort = string.IsNullOrEmpty(SortOrder) ? "name_desc" : "";
                PriceSort = SortOrder == "price" ? "price_desc" : "price";
                DateSort = SortOrder == "date" ? "date_desc" : "date";
                DurationSort = SortOrder == "duration" ? "duration_desc" : "duration";

                // Fetch all plans via the service
                var plans = await _membershipPlanService.GetAllAsync();

                // Filter by search string if provided
                if (!string.IsNullOrEmpty(SearchString))
                {
                    plans = plans.Where(p =>
                        p.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)
                        || (p.Description != null && p.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
                }

                // Apply Active filter if requested
                if (ActiveOnly == true)
                {
                    plans = plans.Where(p => p.IsActive == true);
                }

                // Apply sorting (example using LINQ in memory)
                plans = SortOrder switch
                {
                    "name_desc" => plans.OrderByDescending(p => p.Name),
                    "price" => plans.OrderBy(p => p.Price),
                    "price_desc" => plans.OrderByDescending(p => p.Price),
                    "date" => plans.OrderBy(p => p.CreatedAt),
                    "date_desc" => plans.OrderByDescending(p => p.CreatedAt),
                    "duration" => plans.OrderBy(p => p.DurationDays),
                    "duration_desc" => plans.OrderByDescending(p => p.DurationDays),
                    _ => plans.OrderBy(p => p.Name)
                };

                var planList = plans.ToList();

                // Calculate statistics
                TotalPlans = planList.Count;
                ActivePlans = planList.Count(p => p.IsActive == true);
                AveragePrice = TotalPlans > 0 ? planList.Average(p => p.Price) : 0;

                // If using pagination helper, for example:
                int pageSize = 10;
                PaginatedPlans = await PaginatedList<MSSQLServer.EntitiesModels.MembershipPlan>.CreateAsync(planList.AsQueryable(), PageNumber ?? 1, pageSize);

                // Set the membership plans (for backward compatibility if needed)
                MembershipPlans = PaginatedPlans;

                _logger.LogInformation("Membership plans retrieved successfully. Count: {Count}", planList.Count);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving membership plans via service");
                StatusMessage = "Error: Failed to retrieve membership plans.";
                MembershipPlans = new List<MSSQLServer.EntitiesModels.MembershipPlan>();
                return Page();
            }
        }
    }

    // Pagination helper class remains unchanged
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return await Task.FromResult(new PaginatedList<T>(items, count, pageIndex, pageSize));
        }
    }
}
