using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProjectManager.Models
{
    public class AdditionalFiles
    {
        [Key]
        public int Id { get; set; }
        public List<FilePath> Files { get; set; }
    }
}
