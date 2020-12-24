using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Diploma.Models;
using Diploma.Models.DataBase;

namespace Diploma.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdmitsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admits
        public ActionResult Index()
        {
            var admits = db.Admits.Include(a => a.Room);
            return View(admits.ToList());
        }

        // GET: Admits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admit admit = db.Admits.Find(id);
            if (admit == null)
            {
                return HttpNotFound();
            }
            return View(admit);
        }

        // GET: Admits/Create
        public ActionResult Create()
        {
            ViewBag.RoomsId = new SelectList(db.Rooms, "Id", "Id");
            return View();
        }

        // POST: Admits/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Start,End,RoomsId")] Admit admit)
        {
            if (ModelState.IsValid)
            {
                db.Admits.Add(admit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomsId = new SelectList(db.Rooms, "Id", "Id", admit.RoomsId);
            return View(admit);
        }

        // GET: Admits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admit admit = db.Admits.Find(id);
            if (admit == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomsId = new SelectList(db.Rooms, "Id", "Id", admit.RoomsId);
            return View(admit);
        }

        // POST: Admits/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Start,End,RoomsId")] Admit admit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomsId = new SelectList(db.Rooms, "Id", "Id", admit.RoomsId);
            return View(admit);
        }

        // GET: Admits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admit admit = db.Admits.Find(id);
            if (admit == null)
            {
                return HttpNotFound();
            }
            return View(admit);
        }

        // POST: Admits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admit admit = db.Admits.Find(id);
            db.Admits.Remove(admit);
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
