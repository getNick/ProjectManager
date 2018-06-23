using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Areas.Scrum.ViewModels;
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
        private IHostingEnvironment _appEnvironment;
        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHostingEnvironment appEnvironment)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
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
                var participants = _db.Participants.Include(x => x.Team).Include(x => x.User).Include(x => x.Project).ThenInclude(x => x.Tasks).Where(x => x.User.Id == userId).ToList();
                if (participants.Count > 1)
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
                participant = _db.Participants.Include(x=>x.Team).Include(x => x.User).Include(x => x.Project).FirstOrDefault(x => (x.User.Id == userId) & (x.Project.Id == user.LastSelectedProjectId));
            }
            var vm = new TaskPageViewModel();
            var AllMyTask = _db.Tasks.Include(x => x.Assignee).ThenInclude(x => x.User)
                .Where(x => x.Assignee.Id == participant.Id).ToList();
            vm.MyOpenTask = AllMyTask
                .Where(x =>x.Status != TaskStatusEnum.Done).ToList();
            vm.AllMyClosedTasks = AllMyTask
                .Where(x => x.Status == TaskStatusEnum.Done).ToList();
            vm.CanCreateTask = participant.Role == RoleEnum.Manager;

            var data = _db.Departments.Include(x => x.Teams).ThenInclude(x => x.Participants).ThenInclude(x => x.User).Where(x => x.Project.Id == participant.Project.Id).ToList();
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
                        ID = department.Id + "_" + team.Id,
                        CategoryId = department.Id.ToString(),
                        Text = team.Name,
                        Expanded = true,
                    });
                    foreach (var teamParticipant in team.Participants)
                    {
                        vm.SelectorData.Add(new SelectorItem()
                        {
                            ID = department.Id + "_" + team.Id + "_" + teamParticipant.Id,
                            CategoryId = department.Id + "_" + team.Id,
                            Text = teamParticipant.User.FullName,
                        });
                    }
                }
            }
           
            vm.iCusomer = participant.Role == RoleEnum.Customer;
            if (vm.iCusomer | participant.Role == RoleEnum.Manager)
            {
                vm.CustomerWishes = _db.CustomerWishes.Include(x => x.Project).Include(x => x.Autor).ThenInclude(x=>x.User)
                    .Where(x => x.Project.Id == participant.Project.Id).ToList();
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
                    }
                    break;
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
            vm.CustomList = allTasks;

            //if (vm.CustomList == null)
            //{

            //    vm.CustomList = _db.Tasks.Include(x => x.Project).Include(x => x.Assignee).ThenInclude(x => x.User)
            //        .Where(x => x.Project.Id == user.LastSelectedProjectId).ToList();
            //}
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
            return View(vm);
        }
        public IActionResult Edit(int? taskId)
        {
            var task = _db.Tasks.Include("AdditionalFiles.Files").Include(x => x.WorkPeriods).Include("Activities.List")
                .Include("Comments.List.Autor.User").Include(x => x.BaseTask).Include("Assignee.User").Include(x => x.Team).Include(x => x.Subtasks).FirstOrDefault(x => x.Id == taskId);
            var vm = new EditTaskViewModel();
            vm.Task = task;
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            var participant = _db.Participants.Include(x=>x.Team).Include(x => x.Project).ThenInclude(x => x.Departments).ThenInclude(x => x.Teams).ThenInclude(x => x.Participants).
                ThenInclude(x => x.User).FirstOrDefault(x => (x.Project.Id == user.LastSelectedProjectId) & (x.User.Id == user.Id));
            if ((task.Assignee != null) && (task.Assignee.Id == participant.Id))
            {
                vm.IsMyTask = true;
                var activePeriod = task.WorkPeriods.FirstOrDefault(x => x.Finished == false);
                if (activePeriod != null)
                {
                    vm.HaveActivePeriod = true;
                }
                vm.AlreadySpentTime = task.WorkPeriods.Where(x => x.Finished)
                    .Sum(x => (x.End.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds) - (x.Start.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds));
                 vm.AlreadySpentTime+= activePeriod != null ? DateTime.Now.Subtract(activePeriod.Start).TotalMilliseconds : 0;

            }
            else if (task.Team.Id == participant?.Team?.Id)
            {
                vm.CanIAssingThisTask = true;
            }
            return View(vm);
        }
        public IActionResult Save(int?taskId, string description, string status, int? minTime,int? maxTime,int complitedLine,string newComment)
        {
            var task = _db.Tasks.Include("AdditionalFiles.Files").Include(x => x.WorkPeriods).Include("Activities.List")
                .Include("Comments.List").Include(x => x.BaseTask).Include("Assignee.User").Include(x => x.Subtasks).FirstOrDefault(x => x.Id == taskId);
            if (task.AdditionalFiles == null)
            {
                task.AdditionalFiles=new AdditionalFiles()
                {
                    Files = new List<FilePath>(),
                };
            }
            var myFiles = Request.Form.Files;
            var targetLocation = Path.Combine(_appEnvironment.WebRootPath, "images");
            foreach (var file in myFiles)
            {
                if (file.Length > 0)
                {
                    var path = Path.Combine(targetLocation, file.FileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        file.CopyTo(fileStream);
                    }
                    task.AdditionalFiles.Files.Add(new FilePath() { Owner = task.AdditionalFiles, Path = file.FileName });
                }
            }

            if (description != null)
            {
                task.Description = description;
            }

            task.Status = (TaskStatusEnum) Enum.Parse(typeof(TaskStatusEnum), status);
            if (minTime != null)
            {
                task.MinEstimate = new TimeSpan((int)minTime, 0, 0);
            }
            if (maxTime != null)
            {
                task.MaxEstimate = new TimeSpan((int)maxTime, 0, 0);
            }

            task.ComplitedLine = complitedLine;
            if (newComment != null)
            {
                if (task.Comments == null)
                {
                    task.Comments = new Comments()
                    {
                        List = new List<Comment>(),
                    };
                }

                var userId = _userManager.GetUserId(HttpContext.User);
                var user = _db.Users.FirstOrDefault(x => x.Id == userId);
                var participant = _db.Participants.FirstOrDefault(x =>
                    (x.Project.Id == user.LastSelectedProjectId) & (x.User.Id == user.Id));
                task.Comments.List.Add(new Comment()
                {
                    Autor = participant,
                    Time = DateTime.Now,
                    Description = newComment,
                });
            }

            _db.Tasks.Update(task);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Start(int? taskId)
        {
            var task = _db.Tasks.Include(x => x.WorkPeriods).FirstOrDefault(x => x.Id == taskId);
            task.WorkPeriods.Add(new WorkPeriod()
            {
                Start = DateTime.Now,
                Finished = false,
            });
            _db.Tasks.Update(task);
            _db.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult Stop(int? taskId)
        {
            var task = _db.Tasks.Include(x => x.WorkPeriods).FirstOrDefault(x => x.Id == taskId);
            var workPeriod=task.WorkPeriods.FirstOrDefault(x => x.Finished == false);
            workPeriod.End=DateTime.Now;
            workPeriod.Finished = true;
            _db.Tasks.Update(task);
            _db.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult SaveNewTask(int? assigneId,int? departmentId,int? teamId, string name,string description,string priority,string taskType,string deadline)
        {
            var newTask = new ProjectTask()
            {
                Name = name,
                Description = description,
                Status = TaskStatusEnum.Backlog,
                Assignee = _db.Participants.FirstOrDefault(x=>x.Id==assigneId),
                Department = _db.Departments.Include(x=>x.Project).FirstOrDefault(x=>x.Id==departmentId),
                Team = _db.Teams.FirstOrDefault(x=>x.Id==teamId),
                AddingTime = DateTime.Now,
                ComplitedLine = 0,
                Deadline = DateTime.Parse(deadline),
                Priority = (PriorityEnum)Enum.Parse(typeof(PriorityEnum),priority,true),
                Type = (TaskTypeEnum)Enum.Parse(typeof(TaskTypeEnum), taskType, true),
                
            };
            newTask.Project = newTask.Department.Project;
            var files = new AdditionalFiles {Files = new List<FilePath>()};
            var myFiles = Request.Form.Files;
            var targetLocation = Path.Combine(_appEnvironment.WebRootPath, "images");
            foreach (var file in myFiles)
            {
                if (file.Length > 0)
                {
                    var path = Path.Combine(targetLocation, file.FileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        file.CopyTo(fileStream);
                    }
                    files.Files.Add(new FilePath() { Owner = files, Path = file.FileName });
                }
            }
            newTask.AdditionalFiles = files;
            _db.Tasks.Add(newTask);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Show(int? taskId)
        {
            var task = _db.Tasks.Include(x => x.AdditionalFiles).ThenInclude(x => x.Files).Include(x => x.Subtasks)
                .Include(x => x.Assignee).Include(x => x.WorkPeriods);
            return View();
        }
        public IActionResult Delete(int? taskId)
        {
            return View();
        }
        public IActionResult AssignForMe(int? taskId)
        {
            var task = _db.Tasks.FirstOrDefault(x => x.Id == taskId);
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            var participant = _db.Participants.Include(x => x.Team).ThenInclude(x => x.Tasks).ThenInclude(x => x.Assignee).ThenInclude(x => x.User).Include(x => x.Department).ThenInclude(x => x.Tasks).ThenInclude(x => x.Assignee).ThenInclude(x => x.User)
                .Include(x => x.Project).ThenInclude(x => x.Tasks).ThenInclude(x => x.Assignee).ThenInclude(x => x.User).FirstOrDefault(x =>
                    (x.Project.Id == user.LastSelectedProjectId) & (x.User.Id == user.Id));
            task.Assignee = participant;
            _db.Tasks.Update(task);
            _db.SaveChanges();
            return RedirectToAction("Edit", "Task", new { taskId = task.Id });
        }

        public IActionResult AddCustomerWish(string name, string description,string deadline)
        {
            if (name == null & description == null & deadline == null) return View("CreateCustomerWish");
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            var participant = _db.Participants.Include(x => x.Team).ThenInclude(x => x.Tasks).ThenInclude(x => x.Assignee).ThenInclude(x => x.User).Include(x => x.Department).ThenInclude(x => x.Tasks).ThenInclude(x => x.Assignee).ThenInclude(x => x.User)
                .Include(x => x.Project).ThenInclude(x => x.Tasks).ThenInclude(x => x.Assignee).ThenInclude(x => x.User).FirstOrDefault(x =>
                    (x.Project.Id == user.LastSelectedProjectId) & (x.User.Id == user.Id));
            var newTask = new CustomerWish()
            {
                Name = name,
                Description = description,
                Autor = participant,
                AddingTime = DateTime.Now,
                Deadline = DateTime.Parse(deadline),
            };
            newTask.Project = participant.Project;
            var files = new AdditionalFiles { Files = new List<FilePath>() };
            var myFiles = Request.Form.Files;
            var targetLocation = Path.Combine(_appEnvironment.WebRootPath, "images");
            foreach (var file in myFiles)
            {
                if (file.Length > 0)
                {
                    var path = Path.Combine(targetLocation, file.FileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        file.CopyTo(fileStream);
                    }
                    files.Files.Add(new FilePath() { Owner = files, Path = file.FileName });
                }
            }
            newTask.AdditionalFiles = files;
            _db.CustomerWishes.Add(newTask);
            _db.SaveChanges();
            return View("Index");
        }
    }
}