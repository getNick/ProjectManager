using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.ProjectViewModels
{
    public class EditProjectViewModel
    {
        public Project Project { get; set; }
        public int? SelectedDepartmentId { get; set; }
        public int? SelectedTeamId { get; set; }
        public List<Participant> Customers { get; set; }
    }
}
