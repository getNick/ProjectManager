using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Models.ConstAndEnums;
using ProjectManager.Models.DashboardViewModels;

namespace ProjectManager.Controllers
{
    public class TaskController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _db;

        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            var participant = _db.Participants.Include(x=>x.Team).FirstOrDefault(x =>
                (x.Project.Id == user.LastSelectedProjectId) & (x.User.Id == user.Id));
            //var dep = _db.Departments.Include(x => x.Teams).ThenInclude(x => x.Tasks).ThenInclude(x => x.Assignee)
            //    .FirstOrDefault(x => x.Project.Id == projId);
            var vm = new TaskPageViewModel();
            if (participant.Team != null)
            {
                var team = _db.Teams.Include(x => x.Tasks).FirstOrDefault(x => x.Id == participant.Team.Id);
                var teamTasks = team.Tasks;
                vm.MyOpenTask = teamTasks
                    .Where(x => (x.Assignee.Id == participant.Id) & (x.Status != TaskStatusEnum.Done)).ToList();
                vm.AllMyClosedTasks = teamTasks
                    .Where(x => (x.Assignee.Id == participant.Id) & (x.Status == TaskStatusEnum.Done)).ToList();
                vm.MyTeamTasks = teamTasks;
            }
            vm.CanCreateTask = participant.Role == RoleEnum.Manager;
            return View(vm);
        }

        public IActionResult Create(int? selectedDepartmentId, int?selectedTeamId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            var participant = _db.Participants.Include(x=>x.Project).ThenInclude(x=>x.Departments).ThenInclude(x=>x.Teams).ThenInclude(x=>x.Participants).
                ThenInclude(x=>x.User).FirstOrDefault(x =>(x.Project.Id == user.LastSelectedProjectId) & (x.User.Id == user.Id));
            CreateNewTaskViewModel vm = new CreateNewTaskViewModel();

            vm.AllDepartments = participant.Project.Departments;
            vm.SelectedDepartment = (selectedDepartmentId == null
                                        ? participant.Department
                                        : participant.Project.Departments.FirstOrDefault(x => x.Id == selectedDepartmentId)) ??
                                    vm.AllDepartments.FirstOrDefault();
            vm.AllTeams = vm.SelectedDepartment?.Teams;
            vm.SelectedTeam = (selectedTeamId == null
                                  ? participant.Team
                                  : _db.Teams.Include(x=>x.Participants).FirstOrDefault(x=>x.Id==selectedTeamId)) ?? 
                              vm.AllTeams.FirstOrDefault();
            //vm.SelectedDepartment = selectedDepartmentId == null ? vm.AllDepartments.FirstOrDefault(x => x.Teams.Contains(user.Team)) : vm.AllDepartments.FirstOrDefault(x => x.Id == selectedDepartmentId);

            return View(vm);
        }
        public IActionResult Edit(int? taskId)
        {
            return View();
        }
        public IActionResult Save(int? taskId)
        {
            return View();
        }

        public IActionResult Show(int? taskId)
        {
            return View();
        }
        public IActionResult Delete(int? taskId)
        {
            return View();
        }
    }
}