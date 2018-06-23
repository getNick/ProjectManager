using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.ProductKnowledge
{
    public class Expression
    {
        [Key]
        public int Id { get; set; }
        public Variable LeftVariable { get; set; }
        public Ratio Ratio { get; set; }
        public string RightVariable { get; set; }
    }
}
