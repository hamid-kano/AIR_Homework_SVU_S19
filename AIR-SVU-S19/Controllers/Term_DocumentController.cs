using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AIR_SVU_S19;
using AIR_SVU_S19.Models;

namespace AIR_SVU_S19.Controllers
{
    public class Term_DocumentController : Controller
    {
        private AIR_SVU_S19_Model db = new AIR_SVU_S19_Model();

        // GET: Term_Document
        public ActionResult Index()
        {
            return View(db.Term_Document.ToList());
        }

        // GET: Term_Document/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term_Document term_Document = db.Term_Document.Find(id);
            if (term_Document == null)
            {
                return HttpNotFound();
            }
            return View(term_Document);
        }

        // GET: Term_Document/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Term_Document/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Terms,Docs,Freg_Term_in_docs")] Term_Document term_Document)
        {
            if (ModelState.IsValid)
            {
                db.Term_Document.Add(term_Document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(term_Document);
        }

        // GET: Term_Document/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term_Document term_Document = db.Term_Document.Find(id);
            if (term_Document == null)
            {
                return HttpNotFound();
            }
            return View(term_Document);
        }

        // POST: Term_Document/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Terms,Docs,Freg_Term_in_docs")] Term_Document term_Document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(term_Document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(term_Document);
        }

        // GET: Term_Document/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term_Document term_Document = db.Term_Document.Find(id);
            if (term_Document == null)
            {
                return HttpNotFound();
            }
            return View(term_Document);
        }

        // POST: Term_Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Term_Document term_Document = db.Term_Document.Find(id);
            db.Term_Document.Remove(term_Document);
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
