using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response
{
    public class WorkoutPlanResponse
    {
        public List<WorkoutPlan> WorkoutPlan { get; set; } = new();
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
    }
}
