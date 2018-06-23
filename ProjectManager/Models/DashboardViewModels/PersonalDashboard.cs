using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.DashboardViewModels
{
    public class PersonalDashboard
    {
        //public List<Department> AllDepartments { get; set; }
        //public Department SelectedDepartment { get; set; }
        //public List<Team> AllTeams { get; set; }
        //public Team SelectedTeam { get; set; }
        //public List<Participant> AllParticipants { get; set; }
        //public Participant SelectedParticipant { get; set; }
        //public Project ActiveProject { get; set; }
        public List<ProjectTask> Backlog { get; set; }
        public List<ProjectTask> ToDo { get; set; }
        public List<ProjectTask> InProgress { get; set; }
        public List<ProjectTask> Testing { get; set; }
        public List<ProjectTask> Done { get; set; }
        public List<Object> SelectorData { get; set; }
        public string SelectorValue { get; set; }
    }
}
