using AIR_SVU_S19.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Word = Microsoft.Office.Interop.Word;

namespace AIR_SVU_S19.Controllers
{
    public class HomeController : Controller
    {
        AIR_SVU_S19_Model db = new AIR_SVU_S19_Model();
        public static Dictionary<string, List<string>> documentCollection = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<int>> termDocumentIncidenceMatrix = new Dictionary<string, List<int>>();
        public static HashSet<string> distinctTerm = new HashSet<string>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Relevant_Docs()
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
                if (db.Files.Where(f=>f.File_Name.Equals(fileName)).SingleOrDefault()==null)
                {
                file.SaveAs(Server.MapPath("~/Files/") + fileName); //File will be saved in application root
                Files _file = new Files();
                _file.File_Name = Server.MapPath("~/Files/") + fileName;
                _file.File_content =null;
                db.Files.Add(_file);
                db.SaveChanges();
                }
            }
            return Json("Uploaded " + Request.Files.Count + " files");
        }
        
        [HttpPost]
        public ActionResult Mainpulation_Text(FormCollection values)
        {
            #region enter_stemming_term_with_Docs
            string langSelect = values["Lang"];
            string AlgorithmSelect = values["Algorithm"];
            HashSet<string> hashSet = new HashSet<string>();
            Word.Application app = new Word.Application();
            Word.Document doc;
            object missing = Type.Missing;
            object readOnly = true;
            string poureText;
            IList<Files> Files_List = db.Files.ToList();
            bool insertNewDoc = false;
            foreach (var file in Files_List)
            {
               
                if (file.File_content == null)
                {
                    insertNewDoc = true;
                    object path = file.File_Name;// @"C:\Users\حميد عبيد\source\repos\AIR-SVU-S19\AIR-SVU-S19\Files\1.doc";// file;
                    doc = app.Documents.Open(ref path, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    string text = doc.Content.Text;
                    Files _file = db.Files.Find(file.File_ID);  //db.Files.Find(file.File_ID);//  .Where(u => u.File_Name.Equals(file)).SingleOrDefault();
                    _file.File_Name = doc.Name;// file.File_Name;
                    _file.File_content = text;
                    db.Entry(_file).State = EntityState.Modified;
                    db.SaveChanges();
                    doc.Close();
                    poureText = Stopword_Arabic.RemoveStopwords(text);
                    poureText = Stopword_English.RemoveStopwords(poureText);
                    poureText = ReturnCleanASCII(poureText);
                    string wordStemm;
                    hashSet = new HashSet<string>((from a in db.Term_Document select a.Terms).Distinct());
                    int countTerm = hashSet.Count;
                    if (langSelect== "arabic")
                    {
                        ISRI_Stemmer_Arabic stemmerArabic = new ISRI_Stemmer_Arabic();
                        foreach (var item in poureText.Split(' '))
                        {
                            Term_Document newTerm = new Term_Document();
                            wordStemm = stemmerArabic.Stemming(item);
                            if (wordStemm.Length>2)
                            hashSet.Add(wordStemm);
                            if (countTerm<hashSet.Count)
                            {
                                countTerm++;
                                newTerm.Terms = wordStemm;
                                newTerm.Docs = file.File_Name + " ";
                                db.Term_Document.Add(newTerm);
                                db.SaveChanges();
                            }
                            else
                            {
                                newTerm = db.Term_Document.Where(f => f.Terms.Equals(wordStemm)).FirstOrDefault();
                              if (newTerm != null)
                              {
                                    var listDocs = newTerm.Docs.Split(' ');
                                    if (!listDocs.Contains(file.File_Name))
                                    {
                                    newTerm.Docs += file.File_Name + " ";
                                    db.Entry(newTerm).State = EntityState.Modified;
                                    db.SaveChanges();
                                    }
                              }
                            }
                        }
                    }
                    else
                    {
                        Porter_Stemmer_English stemmerEnglish = new Porter_Stemmer_English();
                        foreach (var item in poureText.Split(' '))
                        {
                            Term_Document newTerm = new Term_Document();
                            wordStemm = stemmerEnglish.stem(item);
                            if (wordStemm.Length > 2)
                                hashSet.Add(wordStemm);
                            if (countTerm < hashSet.Count)
                            {
                                countTerm++;
                                newTerm.Terms = wordStemm;
                                newTerm.Docs = file.File_Name + " ";
                                db.Term_Document.Add(newTerm);
                                db.SaveChanges();
                            }
                            else
                            {
                                newTerm = db.Term_Document.Where(f => f.Terms.Equals(wordStemm)).FirstOrDefault();
                                if (newTerm != null)
                                {
                                    var listDocs = newTerm.Docs.Split(' ');
                                    if (!listDocs.Contains(file.File_Name))
                                    {
                                        newTerm.Docs += file.File_Name + " ";
                                        db.Entry(newTerm).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion enter_stemming_term_with_Docs
            if (insertNewDoc)
            {
                foreach (var item in db.OrderTerms_DocsBoolean)
                {
                    db.OrderTerms_DocsBoolean.Remove(item);
                }
                db.SaveChanges();
                documentCollection = new Dictionary<string, List<string>>();
                List<string> listOfDocument = new List<string>();
                listOfDocument = db.Files.Select(f => f.File_Name).ToList<string>();
                foreach (var file in listOfDocument)
                {
                    List<string> listTermDoc = db.Term_Document.Where(d => d.Docs.Contains(file)).Select(t => t.Terms).ToList<string>();
                    documentCollection.Add(file, listTermDoc);
                }
                distinctTerm = db.Term_Document.Select(x => x.Terms).Distinct().ToHashSet<string>();
                termDocumentIncidenceMatrix = BooleanQueryManipulationClass.GetTermDocumentIncidenceMatrix(distinctTerm, documentCollection);
                insert_Distinct_Term_Docs_To_DB(termDocumentIncidenceMatrix);

            }
            return RedirectToAction("Index");
        }
        public string ReturnCleanASCII(string s)
        {
            StringBuilder sb = new StringBuilder(s.Length);
           foreach (char c in s)
            {
                if (c == 'ـ')
                continue;
                if (Char.IsLetterOrDigit(c))
                sb.Append(c);
                else
                sb.Append(' ');
            }
            return sb.ToString();
        }
        public JsonResult GetSearchingData(string SearchBy, string SearchValue ,string SearchModel)
        {
            Porter_Stemmer_English porterStem = new Porter_Stemmer_English();
            string stermWordQuery = porterStem.stem(SearchValue);
            List<Files> filesList = new List<Files>();
            if (SearchValue=="")
            {
                return Json(db.Files.ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                var listFiles = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToUpper().Equals(stermWordQuery.ToUpper()) || t.Term.ToUpper().Equals(SearchValue.ToUpper())).FirstOrDefault();
                if (listFiles!=null)
                {
                    int index = 0;
                    foreach (var item in listFiles.Docs.Split(' '))
                    {
                        if (item == "1")
                        {
                            filesList.Add(db.Files.ToList()[index]);
                        }
                        index++;
                    }
                    return Json(filesList.ToList(), JsonRequestBehavior.AllowGet);
                }
                return null;
            }
           // return null;
        }
        public void insert_Distinct_Term_Docs_To_DB(Dictionary<string, List<int>> termDocumentIncidenceMatrix)
        {
            AIR_SVU_S19_Model db = new AIR_SVU_S19_Model();
            string output = "";
            foreach (var item in termDocumentIncidenceMatrix)
            {
                OrderTerms_DocsBoolean orederTerm = new OrderTerms_DocsBoolean();
                orederTerm.Term = item.Key;
                foreach (var value in item.Value)
                {
                    output += value+" ";
                }
                orederTerm.Docs= output;
                db.OrderTerms_DocsBoolean.Add(orederTerm);
                db.SaveChanges();
                output = "";
            }
            //System.IO.File.WriteAllText(@"D:\hamid.txt", output);

        }
    }
}