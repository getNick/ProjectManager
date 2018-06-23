using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;

namespace ProjectManager.Models
{
    public class Selector
    {
        private ApplicationDbContext _db;

        public Selector(ApplicationDbContext context)
        {
            _db = context;
        }

        public List<object> SelectDepartmentTeamPart(int projId)
        {
            
            var data = _db.Departments.Include(x => x.Teams).ThenInclude(x => x.Participants).ThenInclude(x => x.User).Where(x => x.Project.Id == projId).ToList();
            var selector= new List<object>();
            foreach (var department in data)
            {
                selector.Add(new SelectorItem()
                {
                    ID = department.Id.ToString(),
                    Text = department.Name,
                    Expanded = true,
                });
                foreach (var team in department.Teams)
                {
                    selector.Add(new SelectorItem()
                    {
                        ID = department.Id + "_" + team.Id,
                        CategoryId = department.Id.ToString(),
                        Text = team.Name,
                        Expanded = true,
                    });
                    foreach (var teamParticipant in team.Participants)
                    {
                        selector.Add(new SelectorItem()
                        {
                            ID = department.Id + "_" + team.Id + "_" + teamParticipant.Id,
                            CategoryId = department.Id + "_" + team.Id,
                            Text = teamParticipant.User.FullName,
                        });
                    }
                }
            }

            return selector;
        }

    }
}
