using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.DashboardViewModels
{
    public class TaskPageViewModel
    {
        [Display(Name = "My open tasks")]
        public List<ProjectTask> MyOpenTask { get; set; }
        [Display(Name = "My team tasks")]
        public List<ProjectTask> MyTeamTasks { get; set; }
        [Display(Name = "My department tasks")]
        public List<ProjectTask> MyDepartmentTasks { get; set; }
        [Display(Name = "My project tasks")]
        public List<ProjectTask> MyProjectTasks { get; set; }
        [Display(Name = "All my closed tasks")]
        public List<ProjectTask> AllMyClosedTasks { get; set; }
        public bool CanCreateTask { get; set; }
        [Display(Name = "All departments")]
        public List<Department> AllDepartments { get; set; }
        public int? SelectedDepartmentId { get; set; }
        [Display(Name = "All teams")]
        public List<Team> AllTeams { get; set; }
        public int? SelectedTeamId { get; set; }
        public List<ProjectTask> CustomList { get; set; }
    }
}
