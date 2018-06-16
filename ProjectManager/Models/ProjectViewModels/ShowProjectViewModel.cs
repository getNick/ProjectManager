using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.ProjectViewModels
{
    public class ShowProjectViewModel
    {
        public List<Project> AllProjects { get; set; }
        public Project SelectedProject { get; set; }
    }
}
