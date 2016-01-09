using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NERCManagementSystem.Models;

namespace NERCManagementSystem.Controllers
{
    [Authorize]
    public class InstitutionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var institutions = db.Institutions.Include(i => i.City);
            return View(institutions.ToList());
        }

        [Authorize(Roles = "Admin, Organizer")]
        public ActionResult Add()
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include="Name,CityID")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                db.Institutions.Add(institution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Cities, "ID", "Name", institution.CityID);
            return View(institution);
        }

        // GET: /Institutions/Edit/5
        [Authorize(Roles = "Admin, Organizer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "Name", institution.CityID);
            return View(institution);
        }

        // POST: /Institutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,CityID")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(institution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "Name", institution.CityID);
            return View(institution);
        }

        // GET: /Institutions/Delete/5
        [Authorize(Roles = "Admin, Organizer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            return View(institution);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Institution institution = db.Institutions.Find(id);
            db.Institutions.Remove(institution);
            //delete all the related teams and members here
            var teams = db.Teams.Where(team => team.InstitutionID == institution.ID);
            var members = db.Members.Where(member => member.Team.InstitutionID == institution.ID);
            db.Teams.RemoveRange(teams);
            db.Members.RemoveRange(members);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Teams(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            var institutionvm = new InstitutionTeamsViewModel()
            {
                Name = institution.Name,
                City = institution.City,
                Teams = db.Teams.Where(team => team.InstitutionID == institution.ID).ToList()
            };
            return View(institutionvm);
        }

        public ActionResult Members(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            var instvm = new InstitutionMembersVM()
            {
                
                Name = institution.Name,
                City = institution.City,
                Members = db.Members.Where(member => member.Team.InstitutionID == institution.ID).ToList()
            };
            return View(instvm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
