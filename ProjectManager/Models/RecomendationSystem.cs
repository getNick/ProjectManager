using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class RecomendationSystem
    {
        [Display(Name = "Size of team")]
        public int TeamSize { get; set; }
        [Display(Name = "Do you develop an innovative product?")]
        public bool Innovation { get; set; }
        [Display(Name = "Do you have a clear-cut idea of the result of your work?")]
        public bool CrearCutIdea { get; set; }
        [Display(Name = "Is the regular communication of the whole team comfortable")]
        public bool RegularCommunication { get; set; }
        [Display(Name = "The customer is rather a partner and not an investor")]
        public bool CustomerPartner { get; set; }
    }
}
