using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using ThePower.Data.Repository.IRepository;
using ThePower.Models;
using ThePowerData.Data;

namespace ThePowerWeb.Controllers
{
    [Area("Admin")]
    public class MembershipController : Controller
    {
        private readonly IUnitOfWork _db;

        public MembershipController(IUnitOfWork db)
        {
            _db = db;

        }
        public IActionResult GetMemberships()
        {
            IEnumerable<Membership> objs = _db.Membership.GetAll(); 
            return View(objs);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Membership? membership) 
        {
            _db.Membership.Add(membership);
            _db.Save();
            return RedirectToAction("GetMemberships");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var result = _db.Membership.GetFirst(x => x.Id == id);

            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(Membership model)
        {
            _db.Membership.Update(model);
            _db.Save();
            return RedirectToAction("GetMemberships");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = _db.Membership.GetFirst(x => x.Id == id);

            return View(result);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var result = _db.Membership.GetFirst(x => x.Id == id);
            _db.Membership.Remove(result);
            _db.Save();
            return RedirectToAction("GetMemberships");
        }
    }
    
}
