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
    public class MembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Members/
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.MemberType).Include(m => m.Team).OrderBy(m => m.Team.Name);
            return View(members.ToList());
        }

        // GET: /Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: /Members/Create
        [Authorize(Roles="Admin, Organizer")]
        public ActionResult Add()
        {
            ViewBag.MemberTypeID = new SelectList(db.MemberTypes, "ID", "Name");
            ViewBag.TeamID = new SelectList(db.Teams, "ID", "Name");
            return View();
        }

        // POST: /Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include="FirstName,LastName,CNIC,Phone,Email,FatherName,FatherCNIC,FatherPhone,FatherEmail,MemberTypeID,TeamID")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberTypeID = new SelectList(db.MemberTypes, "ID", "Name", member.MemberTypeID);
            ViewBag.TeamID = new SelectList(db.Teams, "ID", "Name", member.TeamID);
            return View(member);
        }

        // GET: /Members/Edit/5
        [Authorize(Roles = "Admin, Organizer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberTypeID = new SelectList(db.MemberTypes, "ID", "Name", member.MemberTypeID);
            ViewBag.TeamID = new SelectList(db.Teams, "ID", "Name", member.TeamID);
            return View(member);
        }

        // POST: /Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,FirstName,LastName,CNIC,Phone,Email,FatherName,FatherCNIC,FatherPhone,FatherEmail,MemberTypeID,TeamID")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberTypeID = new SelectList(db.MemberTypes, "ID", "Name", member.MemberTypeID);
            ViewBag.TeamID = new SelectList(db.Teams, "ID", "Name", member.TeamID);
            return View(member);
        }

        // GET: /Members/Delete/5
        [Authorize(Roles = "Admin, Organizer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: /Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
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
