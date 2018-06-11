using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.DashboardViewModels
{
    public class PersonalDashboard
    {
        public List<ProjectTask> AssignedForMe { get; set; }
        public List<ProjectTask> ConmplitedTasks { get; set; }
    }
}
