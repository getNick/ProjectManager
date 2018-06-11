using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProjectManager.Models
{
    public class FilePath
    {
        [Key]
        public int Id { get; set; }
        public AdditionalFiles Owner { get; set; }
        public string Path { get; set; }
    }
}
