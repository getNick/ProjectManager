using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.ProductKnowledge
{
    public class Rule
    {
        [Key]
        public int Id { get; set; }
        public List<Expression> Expressions { get; set; }
        public Variable RightVariable { get; set; }
        public string Result { get; set; }
    }
}
