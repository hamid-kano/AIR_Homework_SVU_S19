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
    public class OrderTerms_DocsBooleanController : Controller
    {
        private AIR_SVU_S19_Model db = new AIR_SVU_S19_Model();

        // GET: OrderTerms_DocsBoolean
        public ActionResult Index()
        {
            return View(db.OrderTerms_DocsBoolean.ToList());
        }

        // GET: OrderTerms_DocsBoolean/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTerms_DocsBoolean orderTerms_DocsBoolean = db.OrderTerms_DocsBoolean.Find(id);
            if (orderTerms_DocsBoolean == null)
            {
                return HttpNotFound();
            }
            return View(orderTerms_DocsBoolean);
        }

        // GET: OrderTerms_DocsBoolean/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderTerms_DocsBoolean/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Term,Docs")] OrderTerms_DocsBoolean orderTerms_DocsBoolean)
        {
            if (ModelState.IsValid)
            {
                db.OrderTerms_DocsBoolean.Add(orderTerms_DocsBoolean);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderTerms_DocsBoolean);
        }

        // GET: OrderTerms_DocsBoolean/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTerms_DocsBoolean orderTerms_DocsBoolean = db.OrderTerms_DocsBoolean.Find(id);
            if (orderTerms_DocsBoolean == null)
            {
                return HttpNotFound();
            }
            return View(orderTerms_DocsBoolean);
        }

        // POST: OrderTerms_DocsBoolean/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Term,Docs")] OrderTerms_DocsBoolean orderTerms_DocsBoolean)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderTerms_DocsBoolean).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderTerms_DocsBoolean);
        }

        // GET: OrderTerms_DocsBoolean/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTerms_DocsBoolean orderTerms_DocsBoolean = db.OrderTerms_DocsBoolean.Find(id);
            if (orderTerms_DocsBoolean == null)
            {
                return HttpNotFound();
            }
            return View(orderTerms_DocsBoolean);
        }

        // POST: OrderTerms_DocsBoolean/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderTerms_DocsBoolean orderTerms_DocsBoolean = db.OrderTerms_DocsBoolean.Find(id);
            db.OrderTerms_DocsBoolean.Remove(orderTerms_DocsBoolean);
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
