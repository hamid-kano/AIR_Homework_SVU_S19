using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AIR_SVU_S19;
using AIR_SVU_S19.Models;

namespace AIR_SVU_S19.Controllers
{
    public class FilesController : Controller
    {
        private AIR_SVU_S19_Model db = new AIR_SVU_S19_Model();

        // GET: Files
        public ActionResult Index()
        {
            return View(db.Files.ToList());
        }

        // GET: Files/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = db.Files.Find(id);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "File_ID,File_Name,File_content")] Files files)
        {
            if (ModelState.IsValid)
            {
                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc;
                object missing = Type.Missing;
                object readOnly = true;

                //foreach (var file in db.Files.Where(u=>u.File_content==null))
                //{

                object path = @"C:\Users\حميد عبيد\source\repos\AIR-SVU-S19\AIR-SVU-S19\Files\1.doc";// file;
                    //try
                    //{
                           doc = app.Documents.Open(ref path, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                            string text = doc.Content.Text;
                             Files _file = new Files();  //db.Files.Find(file.File_ID);//  .Where(u => u.File_Name.Equals(file)).SingleOrDefault();
                             _file.File_Name = doc.Name;// file.File_Name;
                            _file.File_content = text;
                             db.Files.Add(_file);
                             db.SaveChanges();
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}
                    return RedirectToAction("Index");
            }

            return View(files);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = db.Files.Find(id);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "File_ID,File_Name,File_content")] Files files)
        {
            if (ModelState.IsValid)
            {
                db.Entry(files).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(files);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = db.Files.Find(id);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Files files = db.Files.Find(id);
            db.Files.Remove(files);
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
