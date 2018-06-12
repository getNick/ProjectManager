using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.DashboardViewModels
{
    public class PersonalDashboard
    {
        public Project ActiveProject { get; set; }
        [Display(Name = "Assigned for Me")]
        public List<ProjectTask> AssignedForMe { get; set; }
        [Display(Name = "Completed of this week")]
        public List<ProjectTask> ComplitedTasks { get; set; }
    }
}
