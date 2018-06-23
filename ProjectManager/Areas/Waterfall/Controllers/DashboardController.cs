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

namespace ProjectManager.Waterfall.Controllers
{
    [Area("Waterfall")]
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
        public IActionResult Index(int?selectedProjId)
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
            if (selectedProjId != null)
            {
                user.LastSelectedProjectId = selectedProjId;
                _db.Users.Update(user);
            }
            if (user.LastSelectedProjectId == null)
            {
                var participants = _db.Participants.Include(x => x.User).Include(x => x.Project).ThenInclude(x=>x.Tasks).Where(x => x.User.Id == userId).ToList();
                if (participants.Count> 1)
                {
                    return View("SelectProject", participants);//todo create
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
                participant= _db.Participants.Include(x => x.User).Include(x => x.Project).FirstOrDefault(x => (x.User.Id == userId)&(x.Project.Id==user.LastSelectedProjectId));
            }
           
            return View();
        }
    }
}