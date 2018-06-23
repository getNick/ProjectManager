using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.ProductKnowledge
{
    public class Condition
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public Ratio Ratio { get; set; }
    }
}
