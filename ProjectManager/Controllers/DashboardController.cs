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
    public class DashboardController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index(string selector)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            Participant participant;
            if (user.LastSelectedProjectId == null)
            {
                var participants = _db.Participants.Include(x => x.Team).Include(x => x.User).Include(x => x.Project).ThenInclude(x=>x.Tasks).Where(x => x.User.Id == userId).ToList();
                if (participants.Count> 1)
                {
                    return RedirectToAction("Index", "Project");
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
                participant= _db.Participants.Include(x => x.Team).Include(x => x.User).Include(x => x.Project).FirstOrDefault(x => (x.User.Id == userId)&(x.Project.Id==user.LastSelectedProjectId));
            }

            switch (participant.Project.ProjectType)
            {
                case ProjectTypeEnum.Scrum:
                {
                    return RedirectToAction("Index", "Dashboard", new { Area = "Scrum"});
                }
                case ProjectTypeEnum.Waterfall:
                {
                    return RedirectToAction("Index", "Dashboard", new { Area = "Waterfall"});
                }
            }
            PersonalDashboard vm=new PersonalDashboard();
            var data = _db.Departments.Include(x => x.Teams).ThenInclude(x=>x.Participants).ThenInclude(x=>x.User).Where(x=>x.Project.Id== participant.Project.Id).ToList();
            vm.SelectorData = new List<object>();
            foreach (var department in data)
            {
                vm.SelectorData.Add(new SelectorItem()
                {
                    ID = department.Id.ToString(),
                    Text = department.Name,
                    Expanded = true,
                });
                foreach (var team in department.Teams)
                {
                    vm.SelectorData.Add(new SelectorItem()
                    {
                        ID = department.Id+"_"+team.Id,
                        CategoryId = department.Id.ToString(),
                        Text = team.Name,
                        Expanded = true,
                    });
                    foreach (var teamParticipant in team.Participants)
                    {
                        vm.SelectorData.Add(new SelectorItem()
                        {
                            ID = department.Id+ "_" + team.Id+"_" + teamParticipant.Id,
                            CategoryId = department.Id + "_" + team.Id,
                            Text = teamParticipant.User.FullName,
                        });
                    }
                }
            }

            if (selector == null)
            {
                return View(vm);
            }

            vm.SelectorValue = selector;
            var parseSelector = selector.Split('_');
            List<ProjectTask> allTasks = null;
            switch (parseSelector.Length)
            {
                case 3:
                {
                    allTasks = _db.Tasks.Include(x => x.Assignee).ThenInclude(x => x.User).Include(x => x.Project)
                        .Where(x => (x.Assignee.Id == Int32.Parse(parseSelector[2]))).ToList();
                    } break;
                case 2:
                {
                    allTasks = _db.Tasks.Include(x => x.Assignee).ThenInclude(x => x.User).Include(x => x.Project)
                        .Where(x => (x.Team.Id == Int32.Parse(parseSelector[1]))).ToList();
                    }
                    break;
                case 1:
                {
                    allTasks = _db.Tasks.Include(x => x.Assignee).ThenInclude(x => x.User).Include(x => x.Project)
                        .Where(x => (x.Department.Id == Int32.Parse(parseSelector[0]))).ToList();
                    }
                    break;
            }

            if (allTasks != null)
            {
                vm.Backlog = allTasks.Where(x => x.Status == TaskStatusEnum.Backlog).ToList();
                vm.ToDo = allTasks.Where(x => x.Status == TaskStatusEnum.ToDo).ToList();
                vm.InProgress = allTasks.Where(x => x.Status == TaskStatusEnum.InProgress).ToList();
                vm.Testing = allTasks.Where(x => x.Status == TaskStatusEnum.Testing).ToList();
                vm.Done = allTasks.Where(x => x.Status == TaskStatusEnum.Done).ToList();
            }
            return View(vm);
        }
    }
}