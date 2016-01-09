using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NERCManagementSystem.Models;
using NERCManagementSystem.Controllers;

namespace NERCMS.Controllers
{
    [Authorize]
    public class OrganizersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> usermanager;

        public OrganizersController()
        {
            usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Organizers/
        public ActionResult Index()
        {
            var organizers = db.Organizers.Include(o => o.OrganizerTask).Include(o => o.OrganizerType);
            return View(organizers.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            ViewBag.OrganizerTaskID = new SelectList(db.OrganizerTasks, "ID", "Name");
            ViewBag.OrganizerTypeID = new SelectList(db.OrganizerTypes, "ID", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CreateOrganizerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Username, PasswordHash = (new PasswordHasher()).HashPassword(model.Password) };
                var organizer = new Organizer()
                {
                    ApplicationUserID = user.Id,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    OrganizerTaskID = model.OrganizerTaskID,
                    OrganizerTypeID = model.OrganizerTypeID,
                    Phone = model.Phone,
                    RegNo = model.RegNo
                };
                //usermanager.Create(user);
                //usermanager.AddToRole(user.Id, "Organizer");
                db.Organizers.Add(organizer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizerTaskID = new SelectList(db.OrganizerTasks, "ID", "Name", model.OrganizerTaskID);
            ViewBag.OrganizerTypeID = new SelectList(db.OrganizerTypes, "ID", "Name", model.OrganizerTypeID);

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organizer organizer = db.Organizers.Find(id);
            if (organizer == null)
            {
                return HttpNotFound();
            }
            //if organizer is found
            var user = usermanager.FindById(organizer.ApplicationUserID);
            var organizervm = new EditOrganizerViewModel()
            {
                FirstName = organizer.FirstName,
                LastName = organizer.LastName,
                Email = organizer.Email,
                OrganizerTaskID = organizer.OrganizerTaskID,
                OrganizerTypeID = organizer.OrganizerTypeID,
                Phone = organizer.Phone,
                RegNo = organizer.RegNo,
                Username = user.UserName,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };
            ViewBag.OrganizerTaskID = new SelectList(db.OrganizerTasks, "ID", "Name", organizervm.OrganizerTaskID);
            ViewBag.OrganizerTypeID = new SelectList(db.OrganizerTypes, "ID", "Name", organizervm.OrganizerTypeID);
            return View(organizervm);
        }

        // POST: /Organizers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditOrganizerViewModel organizervm)
        {
            if (ModelState.IsValid)
            {
                var organizer = db.Organizers.Find(id);
                var user = db.Users.Find(organizer.ApplicationUserID);
                HelperFunctions.CopyPropertyValues(organizervm, organizer);
                user.UserName = organizervm.Username;
                if (organizervm.Password != null)
                {
                    usermanager.RemovePassword(user.Id);
                    usermanager.AddPassword(user.Id, organizervm.Password);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizerTaskID = new SelectList(db.OrganizerTasks, "ID", "Name", organizervm.OrganizerTaskID);
            ViewBag.OrganizerTypeID = new SelectList(db.OrganizerTypes, "ID", "Name", organizervm.OrganizerTypeID);
            return View(organizervm);
        }
        // GET: /Organizers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organizer organizer = db.Organizers.Find(id);
            if (organizer == null)
            {
                return HttpNotFound();
            }
            return View(organizer);
        }

        // POST: /Organizers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Organizer organizer = db.Organizers.Find(id);
            var user = usermanager.FindById(organizer.ApplicationUserID);
            foreach (var role in user.Roles)
            {
                usermanager.RemoveFromRole(user.Id, role.ToString());
            }
            usermanager.Delete(user);
            db.Organizers.Remove(organizer);
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
