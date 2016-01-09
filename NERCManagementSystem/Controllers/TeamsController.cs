using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NERCManagementSystem.Models;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NERCManagementSystem.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> usermanager;

        public TeamsController()
        {
            usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Teams/
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.ApplicationUser).Include(t => t.Institution);
            return View(teams.ToList());
        }

        // GET: /Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: /Teams/Create
        [Authorize(Roles = "Admin, Organizer")]
        public ActionResult Add()
        {
            ViewBag.InstitutionID = new SelectList(db.Institutions, "ID", "Name");
            var viewmodel = new AddTeamVM();
            viewmodel.Categories = new List<AddTeamVM.Category>();
            foreach (var category in db.Categories)
            {
                var cat = new AddTeamVM.Category() { CategoryID = category.ID, CategoryName = category.Name, isChecked = false };
                viewmodel.Categories.Add(cat);
            }
            return View(viewmodel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(AddTeamVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, PasswordHash = (new PasswordHasher()).HashPassword(model.Password)};
                var selectedCategories = new List<Category>();
                foreach (var category in model.Categories) {
                    if (category.isChecked) {
                        selectedCategories.Add(db.Categories.Where(c => c.ID == category.CategoryID).First());
                    }
                }
                var team = new Team()
                {
                    Name = model.Name,
                    InstitutionID = model.InstitutionID,
                    ApplicationUserID = user.Id,
                    Categories = selectedCategories
                };
                var result = usermanager.Create(user);
                if (result.Succeeded)
                {
                    usermanager.AddToRole(user.Id, "Team");
                    db.Teams.Add(team);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.InstitutionID = new SelectList(db.Institutions, "ID", "Name");
            model.Categories = new List<AddTeamVM.Category>();
            foreach (var category in db.Categories)
            {
                var cat = new AddTeamVM.Category() { CategoryID = category.ID, CategoryName = category.Name, isChecked = false };
                model.Categories.Add(cat);
            }
            return View(model);
        }

        // GET: /Teams/Edit/5
        [Authorize(Roles = "Admin, Organizer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstitutionID = new SelectList(db.Institutions, "ID", "Name", team.InstitutionID);
            var viewmodel = new EditTeamVM();
            viewmodel.Categories = new List<EditTeamVM.Category>();
            foreach (var category in db.Categories)
            {
                var cat = new EditTeamVM.Category() { CategoryID = category.ID, CategoryName = category.Name, isChecked = false};
                viewmodel.Categories.Add(cat);
            }
            foreach (var category in viewmodel.Categories)
            {
                if (team.Categories.Any(c => c.ID == category.CategoryID))
                {
                    category.isChecked = true;
                }
            }
            viewmodel.Email = team.ApplicationUser.UserName;
            viewmodel.InstitutionID = team.InstitutionID;
            viewmodel.Name = team.Name;
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditTeamVM model)
        {
            if (ModelState.IsValid)
            {
                var team = db.Teams.Find(id);
                team.Name = model.Name;
                team.InstitutionID = model.InstitutionID;
                //remove previous categories
                List<Category> prevCats = team.Categories.ToList();
                foreach (var cat in prevCats)
                {
                    team.Categories.Remove(cat);
                }
                var selectedCategories = new List<Category>();
                foreach (var category in model.Categories)
                {
                    if (category.isChecked)
                    {
                        selectedCategories.Add(db.Categories.Where(c => c.ID == category.CategoryID).First());
                    }
                }
                team.Categories = selectedCategories;
                var user = usermanager.FindById(team.ApplicationUserID);
                user.UserName = model.Email;
                if (model.Password != null)
                {
                    usermanager.RemovePassword(user.Id);
                    usermanager.AddPassword(user.Id, model.Password);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            } 
            ViewBag.InstitutionID = new SelectList(db.Institutions, "ID", "Name", model.InstitutionID);
            var categoriesForVM = new List<EditTeamVM.Category>();
            foreach (var category in db.Categories)
            {
                var cat = new EditTeamVM.Category() { CategoryID = category.ID, CategoryName = category.Name, isChecked = false };
                categoriesForVM.Add(cat);
            }
            foreach (var VMcategory in model.Categories)
            {
                if (VMcategory.isChecked)
                {
                    categoriesForVM.First(c => c.CategoryID == VMcategory.CategoryID).isChecked = true;
                }
            }
            model.Categories = categoriesForVM;
            return View(model);
        }

        // GET: /Teams/Delete/5
        [Authorize(Roles = "Admin, Organizer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            var vm = new DeleteTeamVM() { Name = team.Name, Institution = team.Institution.Name, Categories = team.Categories, NumMembers = team.Members.Count };
            return View(vm);
        }

        // POST: /Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            var members = db.Members.Where(m => m.Team.ID == team.ID);
            db.Teams.Remove(team);
            db.Members.RemoveRange(members);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            if (disposing && usermanager != null)
            {
                usermanager.Dispose();
                usermanager = null;
            }
            base.Dispose(disposing);
        }
    }
}
