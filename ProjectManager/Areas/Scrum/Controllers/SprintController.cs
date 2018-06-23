using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Areas.Scrum.ViewModels;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Areas.Scrum.Controllers
{
    [Area("Scrum")]
    public class SprintController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _db;

        public SprintController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index(int? selectedDepartmentId, int? selectedTeamId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            var part = _db.Participants.Include(x => x.Project).Include(x => x.Department).Include(x => x.Team).Include(x => x.User)
                .ToList();
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            //var projId = HttpContext.Session.GetInt32(SessionKeys.ProjectId);
            Participant participant;

            if (user.LastSelectedProjectId == null)
            {
                var participants = _db.Participants.Include(x => x.Project).Include(x => x.User).Include(x=>x.Project).Include(x=>x.Department)
                    .Include(x=>x.Team).Include(x => x.Project).ThenInclude(x => x.Tasks).Where(x => x.User.Id == userId).ToList();
                if (participants.Count > 1)
                {
                    return RedirectToAction("Index", "Project", new {Area = "Scrum"});
                }

                if (participants.Count == 0)
                {
                    return View("AvailableProjectNotFound");
                }
                participant = participants.FirstOrDefault();
                user.LastSelectedProjectId = participant.Project.Id;
                _db.Users.Update(user);
            }
            else
            {
                participant = _db.Participants.Include(x=>x.Project).Include(x => x.User).Include(x => x.Project).FirstOrDefault(x => (x.User.Id == userId) & (x.Project.Id == user.LastSelectedProjectId));
            }

            var vm = new IndexPageViewModel();
            vm.AllDepartments = _db.Projects.Include(x => x.Departments).ThenInclude(x => x.Teams)
                .FirstOrDefault(x => x.Id == participant.Project.Id).Departments;
            if (selectedDepartmentId != null)
            {
                vm.SelectedDepartment = vm.AllDepartments?.FirstOrDefault(x => x.Id == selectedDepartmentId);
                vm.AllTeams = vm.SelectedDepartment.Teams;
            }
            else
            {
                vm.SelectedDepartment = vm.AllDepartments?.FirstOrDefault();
                vm.AllTeams = vm.SelectedDepartment.Teams;
            }
            if (selectedTeamId != null)
            {
                vm.SelectedTeam = vm.AllTeams.FirstOrDefault(x => x.Id == selectedTeamId);
                var allSprints= _db.Sprints.Include(x => x.ListTasks).ThenInclude(x => x.Assignee).ThenInclude(x => x.User)
                    .Where(x => x.Team.Id == vm.SelectedTeam.Id);
                vm.ActiveSprint = allSprints?.FirstOrDefault(x => x.IsActive);
                vm.NextSprint = allSprints?.FirstOrDefault(x => (x.IsActive == false) & (x.IsFinished == false));
            }
            else
            {
                vm.SelectedTeam = vm.AllTeams.FirstOrDefault();
                if (vm.SelectedTeam != null)
                {
                    var allSprints = _db.Sprints.Include(x => x.ListTasks).ThenInclude(x => x.Assignee).ThenInclude(x => x.User)
                        .Where(x => x.Team.Id == vm.SelectedTeam.Id).ToList();
                    vm.ActiveSprint = allSprints?.FirstOrDefault(x => x.IsActive);
                    vm.NextSprint = allSprints?.FirstOrDefault(x => (x.IsActive == false) & (x.IsFinished == false));
                }
               
            }
            //var allTeamInProj = _db.Projects.Include(x => x.Departments).ThenInclude(x => x.Teams)
            //    .FirstOrDefault(x => x.Id == participant.Project.Id).Departments.SelectMany(x=>x.Teams).ToList();
            
            return View(vm);
        }
        [HttpGet]
        public IActionResult Create(int? teamId)
        {
            var team = _db.Teams.FirstOrDefault(x => x.Id == teamId);
            Sprint sprint=new Sprint()
            {
                Team = team,
                IsActive = false,
                IsFinished = false,
            };
            _db.Sprints.Add(sprint);
            _db.SaveChanges();
            return RedirectToAction("Edit",new{ sprintId=sprint.Id });
        }
        public IActionResult Edit(int? sprintId, string name,string description,int? duration, int[]tasksId)
        {
            var sprint = _db.Sprints.Include(x => x.Team).Include(x => x.ListTasks).FirstOrDefault(x => x.Id == sprintId);
            if (name != null)
            {
                sprint.Name = name;
            }

            if (description != null)
            {
                sprint.Description = description;
            }

            if (duration != null)
            {
                sprint.Duration = (int)duration;
            }

            foreach (var taskId in tasksId)
            {
                if (sprint.ListTasks.FirstOrDefault(x => x.Id == taskId) == null)
                {
                    sprint.ListTasks.Add(_db.Tasks.FirstOrDefault(x=>x.Id==taskId));
                }
            }
            _db.Sprints.Update(sprint);
            _db.SaveChanges();
            return View(sprint);
        }

        public IActionResult Stop(int? sprintId)
        {
            
            return View("Index");
        }
        public IActionResult Start(int? sprintId)
        {
            var sprint = _db.Sprints.Include(x => x.Team).ThenInclude(x=>x.Sprints).Include(x => x.ListTasks).FirstOrDefault(x => x.Id == sprintId);
            var activeSprint = sprint.Team.Sprints.FirstOrDefault(x => x.IsActive);
            if (activeSprint == null)
            {
                sprint.IsActive = true;
                sprint.StartTime=DateTime.Now;
                sprint.Deadline = DateTime.Now.AddDays(sprint.Duration);
                _db.Sprints.Update(sprint);
                _db.SaveChanges();
            }
            else
            {
                View("_CanNotActivateSprint");
            }
            
            return RedirectToAction("Index");
        }
    }
}