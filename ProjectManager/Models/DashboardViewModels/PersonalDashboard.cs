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
        public List<ProjectTask> Backlog { get; set; }
        public List<ProjectTask> ToDo { get; set; }
        public List<ProjectTask> InProgress { get; set; }
        public List<ProjectTask> Testing { get; set; }
        public List<ProjectTask> Done { get; set; }
    }
}
