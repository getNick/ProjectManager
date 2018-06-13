using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Models.ConstAndEnums;

namespace ProjectManager.Models
{
    public class Participant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Project Project { get; set; }
        public Department Department { get; set; }
        public Team Team { get; set; }
        public double Salary { get; set; }
        public RoleEnum Role { get; set; }

    }
}
