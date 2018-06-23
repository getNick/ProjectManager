using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Models;

namespace ProjectManager.Areas.Scrum.ViewModels
{
    public class ReportPageViewModel
    {
        public List<Department> AllDepartments { get; set; }
        public Department SelectedDepartment { get; set; }
        public List<Team> AllTeams { get; set; }
        public Team SelectedTeam { get; set; }
        public List<Participant> AllParticipants { get; set; }
        public Participant SelectedParticipant { get; set; }
        public Sprint ActiveSprint { get; set; }
        public List<object> TaskStatus { get; set; }
        public List<object> WorkPeriods { get; set; }
    }
}
