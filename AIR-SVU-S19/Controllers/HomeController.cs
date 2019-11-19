using AIR_SVU_S19.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Word = Microsoft.Office.Interop.Word;

namespace AIR_SVU_S19.Controllers
{
    public class HomeController : Controller
    {
        AIR_SVU_S19_Model db = new AIR_SVU_S19_Model();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                            //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                file.SaveAs(Server.MapPath("~/Files/") + fileName); //File will be saved in application root
                Files _file = new Files();
                _file.File_Name = Server.MapPath("~/Files/") + fileName;
                _file.File_content =null;
                db.Files.Add(_file);
                db.SaveChanges();
            }
            return Json("Uploaded " + Request.Files.Count + " files");
        }
        
        [HttpPost]
        public ActionResult Mainpulation_Text()
        {
             Word.Application app = new Word.Application();
             Word.Document doc;
            object missing = Type.Missing;
            object readOnly = true;
            IList<Files> Files_List = db.Files.ToList();
            foreach (var file in Files_List)
            {
                if (file.File_content==null)
                {
                    object path =file.File_Name;// @"C:\Users\حميد عبيد\source\repos\AIR-SVU-S19\AIR-SVU-S19\Files\1.doc";// file;
                    doc = app.Documents.Open(ref path, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    string text = doc.Content.Text;
                    Files _file =db.Files.Find(file.File_ID);  //db.Files.Find(file.File_ID);//  .Where(u => u.File_Name.Equals(file)).SingleOrDefault();
                    _file.File_Name = doc.Name;// file.File_Name;
                    _file.File_content = text;
                    db.Entry(_file).State = EntityState.Modified;
                    db.SaveChanges();
                    doc.Close();
                }
            }
            return RedirectToAction("Index");
        }
    }
}