using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Models;

namespace ProjectManager.Areas.Scrum.ViewModels
{
    public class IndexPageViewModel
    {
        public List<Department> AllDepartments { get; set; }
        public Department SelectedDepartment { get; set; }
        public List<Team> AllTeams { get; set; }
        public Team SelectedTeam { get; set; }
        public Sprint ActiveSprint { get; set; }
        public Sprint NextSprint { get; set; }
    }
}
