using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Areas.Scrum.ViewModels;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Models.ConstAndEnums;

namespace ProjectManager.Controllers
{
    public class ReportController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _db;

        public ReportController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(int? selectedDepartmentId, int? selectedTeamId, int? selectedPartId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            //var projId = HttpContext.Session.GetInt32(SessionKeys.ProjectId);
            Participant participant;

            if (user.LastSelectedProjectId == null)
            {
                var participants = _db.Participants.Include(x => x.Project).Include(x => x.User).Include(x => x.Project)
                    .Include(x => x.Department)
                    .Include(x => x.Team).Include(x => x.Project).ThenInclude(x => x.Tasks)
                    .Where(x => x.User.Id == userId).ToList();
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
                participant = _db.Participants.Include(x => x.Project).Include(x => x.User).Include(x => x.Project)
                    .FirstOrDefault(x => (x.User.Id == userId) & (x.Project.Id == user.LastSelectedProjectId));
            }

            var vm = new ReportPageViewModel();
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
                if (vm.SelectedTeam != null)
                {
                    var allSprints = _db.Sprints.Include(x => x.ListTasks).ThenInclude(x => x.Assignee)
                        .ThenInclude(x => x.User).Include(x => x.Team)
                        .Where(x => x.Team.Id == vm.SelectedTeam.Id).ToList();
                    vm.ActiveSprint = allSprints?.FirstOrDefault(x => x.IsActive);
                }
                   
            }
            else
            {
                vm.SelectedTeam = vm.AllTeams.FirstOrDefault();
                if (vm.SelectedTeam != null)
                {
                    var allSprints = _db.Sprints.Include(x => x.ListTasks).ThenInclude(x => x.Assignee)
                        .ThenInclude(x => x.User).Include(x => x.Team)
                        .Where(x => x.Team.Id == vm.SelectedTeam.Id).ToList();
                    vm.ActiveSprint = allSprints?.FirstOrDefault(x => x.IsActive);
                }
               
            }
            //var allTeamInProj = _db.Projects.Include(x => x.Departments).ThenInclude(x => x.Teams)
            //    .FirstOrDefault(x => x.Id == participant.Project.Id).Departments.SelectMany(x=>x.Teams).ToList();
            if (vm.ActiveSprint != null)
            {
                vm.TaskStatus = new List<object>()
                {
                    new {Status = TaskStatusEnum.ToDo.GetDisplayName(), Percent = (vm.ActiveSprint.ListTasks.Count(x=>x.Status==TaskStatusEnum.ToDo)/((double)vm.ActiveSprint.ListTasks.Count))*100},
                    new {Status = TaskStatusEnum.InProgress.GetDisplayName(), Percent = (vm.ActiveSprint.ListTasks.Count(x=>x.Status==TaskStatusEnum.InProgress)/((double)vm.ActiveSprint.ListTasks.Count))*100},
                    new {Status = TaskStatusEnum.Testing.GetDisplayName(), Percent = (vm.ActiveSprint.ListTasks.Count(x=>x.Status==TaskStatusEnum.Testing)/((double)vm.ActiveSprint.ListTasks.Count))*100},
                    new {Status = TaskStatusEnum.Done.GetDisplayName(), Percent = (vm.ActiveSprint.ListTasks.Count(x=>x.Status==TaskStatusEnum.Done)/((double)vm.ActiveSprint.ListTasks.Count))*100},

                };
            }

            //if (vm)
            //{
            vm.WorkPeriods = new List<Object>()
            {
                new
                {
                    text = "Task1",
                    startDate = "2017-06-18T10:58:00.000Z",
                    endDate = "2017-06-18T12:48:00.000Z"
                },
                new
                {  text = "Task1",
                    startDate = "2017-06-18T13:40:00.000Z",
                    endDate = "2017-06-18T16:58:00.000Z"
                },
                new
                {
                    text = "Task1",
                    startDate = "2017-06-18T17:15:00.000Z",
                    endDate = "2017-06-18T18:58:00.000Z"
                },
            };
            //}
           
            return View(vm);
        }
    }
}
