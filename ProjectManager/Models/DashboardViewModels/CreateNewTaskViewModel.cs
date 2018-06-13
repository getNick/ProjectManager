using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.DashboardViewModels
{
    public class CreateNewTaskViewModel
    {
        //public ApplicationUser Creator { get; set; }
        //public List<Project> AllProjects { get; set; }
        public Project Project { get; set; }
        public List<Department> AllDepartments { get; set; }
        public Department SelectedDepartment { get; set; }
        public List<Team> AllTeams { get; set; }
        public Team SelectedTeam { get; set; }
        public bool ThisIsMyTeam { get; set; }
    }
}
