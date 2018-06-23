using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Areas.Waterfall.Models;
using ProjectManager.Models;
using ProjectManager.Models.ProductKnowledge;

namespace ProjectManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<FilePath> FilePaths { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<CustomerWish> CustomerWishes { get; set; }
        public DbSet<Task> GanttTasks { get; set; }
        public DbSet<Link> GanttLinks { get; set; }
        public DbSet<Rule> ProductKnowledgeRules { get; set; }
        public DbSet<Variable> ProductKnowledgeVariables { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Project>().HasOne(x => x.ProjectLead).WithOne(x => x.Project);
            builder.Entity<Project>().HasMany(x => x.Participants).WithOne(x => x.Project);
            //builder.Entity<Department>().HasOne(x => x.HeadOfDepartment).WithOne(x => x.Department);
            builder.Entity<Department>().HasMany(x => x.Participants).WithOne(x => x.Department);
            //builder.Entity<Team>().HasOne(x => x.TeamLeader).WithOne(x => x.Team);
            builder.Entity<Team>().HasMany(x => x.Participants).WithOne(x => x.Team);
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
