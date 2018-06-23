using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Areas.Waterfall.Models;
using ProjectManager.Data;

namespace ProjectManager.Areas.Waterfall.Controllers
{
    public class LinkController : Controller
    {
        private ApplicationDbContext _db;

        public LinkController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET api/Link
        [HttpGet]
        public IEnumerable<LinkDto> Get()
        {
            return _db
                .GanttLinks
                .ToList()
                .Select(l => (LinkDto)l);
        }

        // GET api/Link/5
        [HttpGet]
        public LinkDto Get(int id)
        {
            return (LinkDto)_db
                .GanttLinks
                .Find(id);
        }

        // POST api/Link
        [HttpPost]
        public IActionResult CreateLink(LinkDto linkDto)
        {
            var newLink = (Link)linkDto;
            _db.GanttLinks.Add(newLink);
            _db.SaveChanges();

            return Ok(new
            {
                tid = newLink.Id,
                action = "inserted"
            });
        }

        // PUT api/Link/5
        [HttpPut]
        public IActionResult EditLink(int id, LinkDto linkDto)
        {
            var clientLink = (Link)linkDto;
            clientLink.Id = id;

            _db.Entry(clientLink).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok(new
            {
                action = "updated"
            });
        }

        // DELETE api/Link/5
        [HttpDelete]
        public IActionResult DeleteLink(int id)
        {
            var link = _db.GanttLinks.Find(id);
            if (link != null)
            {
                _db.GanttLinks.Remove(link);
                _db.SaveChanges();
            }
            return Ok(new
            {
                action = "deleted"
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}