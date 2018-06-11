using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class WorkPeriod
    {
        [Key]
        public int Id { get; set; }
        public ProjectTask Task { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Finished { get; set; } = false;
    }
}
