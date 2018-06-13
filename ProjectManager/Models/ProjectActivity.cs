using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class ProjectActivity:BaseModel
    {
        public ApplicationUser Initializer { get; set; }
        public DateTime Time { get; set; }
    }
}
