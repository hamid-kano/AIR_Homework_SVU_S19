using AIR_SVU_S19.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Word = Microsoft.Office.Interop.Word;
using PagedList.Mvc;
using PagedList;

namespace AIR_SVU_S19.Controllers
{
    public class HomeController : Controller
    {
        AIR_SVU_S19_Model db = new AIR_SVU_S19_Model();
        public static Dictionary<string, List<string>> documentCollection = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<int>> termDocumentIncidenceMatrix = new Dictionary<string, List<int>>();
        public static HashSet<string> distinctTerm = new HashSet<string>();
        char[] charsSplit = { ' ' };
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
        public ActionResult ResultSearch(string AlgorithmRetrieve, string TxT_Search_Key,int? i)
        {
            ISRI_Stemmer_Arabic stemmerArabic = new ISRI_Stemmer_Arabic();
            Porter_Stemmer_English StemmerEnglish = new Porter_Stemmer_English();
            string _TextQuery = TxT_Search_Key.ToLower();
            _TextQuery = ReturnCleanASCII(_TextQuery);
            _TextQuery = Stopword_Arabic.RemoveStopwords(_TextQuery);
            _TextQuery = Stopword_English.RemoveStopwords(_TextQuery);
            string StemmingTextQuery = "";
            if (Regex.Matches(_TextQuery, "[a-zA-Z]{2,}").Count >0)
            {
                foreach (var word in _TextQuery.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries))
                {
                    StemmingTextQuery += StemmerEnglish.stem(word) + " ";
                }
            }
            else
            {
                foreach (var word in _TextQuery.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries))
                {
                    StemmingTextQuery += stemmerArabic.Stemming(word) + " ";
                }
            }
                
            List<Files> fileR = new List<Files>();
            Dictionary<string, double> resutlFR;
            if (AlgorithmRetrieve == "vector_space")
            {
                resutlFR = VectorSpace_Model(StemmingTextQuery);
                var sortedDict = from file in resutlFR orderby file.Value descending select file;
                foreach (var item in sortedDict)
                {
                    if (item.Value>0)
                    {
                        fileR.Add(db.Files.Find(Convert.ToInt16(item.Key)));
                    }
                }
            }
            else if (AlgorithmRetrieve == "ex_boolean")
            {
                fileR = Extended_Boolean_Model(StemmingTextQuery);
            }
            else
            {
                fileR= Boolean_Model(StemmingTextQuery);
            }
            Dictionary<int, int> Rank = new Dictionary<int, int>();
            // heigh light queryText  and Compute Ranks to Files
            List<string> TermBoolean = new List<string>() {"and","or","not","أو","او","و","دون" };
            foreach (var item in fileR)
            {
                int RankFiles = 0;
                foreach (var word in TxT_Search_Key.Split(charsSplit,StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!TermBoolean.Contains(word)) 
                    {
                        item.File_content = item.File_content.Replace(word, "<strong style=" + '"' + "background-color: yellow; color: black;" + '"' + ">" + word + "</strong>");
                        RankFiles += Regex.Matches(item.File_content, word).Count; 
                    }
                }
                Rank.Add(item.File_ID, RankFiles);
            }
            var sortedDict2 = from file in Rank orderby file.Value descending select file;
            List<Files> FilesOrderToRank = new List<Files>();
            List<int> rank = new List<int>();
            int AvarageRank = 0;
            if (sortedDict2.Count()!=0)
            {
                int meanRank = sortedDict2.First().Value;
                foreach (var item in sortedDict2) // Create List After order
                {

                    if (item.Value > meanRank - 1)
                    {
                        AvarageRank = 5;
                        rank.Add(AvarageRank);
                        FilesOrderToRank.Add(db.Files.Find(item.Key));
                    }
                    else if (meanRank != 0)
                    {
                        if (item.Value != 0)
                        {
                            AvarageRank = Convert.ToInt32(((double)item.Value / meanRank) * 5);
                            rank.Add(AvarageRank);
                            FilesOrderToRank.Add(db.Files.Find(item.Key));
                        }
                    }
                    else
                    {
                        rank.Add(5);
                        FilesOrderToRank.Add(db.Files.Find(item.Key));
                    }


                }

            }
            ViewBag.ReturnFileCount = FilesOrderToRank.Count;
            ViewBag.RankList = rank.ToList().ToPagedList(i ?? 1,3);
            return View(FilesOrderToRank.ToList().ToPagedList(i ?? 1,3));
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
                if (db.Files.Where(f=>f.File_Name.Equals(fileName)).FirstOrDefault()==null)
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
            string poureText="";
            IList<Files> Files_List = db.Files.ToList();
            bool insertNewDoc = false;
            foreach (var file in Files_List)
            {
                if (file.File_content == null)
                {
                    db.SaveChanges();
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
                    poureText = ReturnCleanASCII(text);
                    //MatchCollection mc = Regex.Matches(text,"\\w+ ") ; // Regex.Escape(text);
                    //foreach (Match mt in mc)
                    //{
                    //    poureText +=" "+ mt.ToString();
                    //}
                    poureText = Stopword_Arabic.RemoveStopwords(poureText);
                    poureText = Stopword_English.RemoveStopwords(poureText);
                    string wordStemm;
                    hashSet = new HashSet<string>((from a in db.Term_Document select a.Terms).Distinct());
                    int countTerm = hashSet.Count;
                    if (langSelect== "arabic")
                    {
                        ISRI_Stemmer_Arabic stemmerArabic = new ISRI_Stemmer_Arabic();
                        foreach (var item in poureText.Split(charsSplit,StringSplitOptions.RemoveEmptyEntries))
                        {
                            Term_Document newTerm = new Term_Document();
                            wordStemm = stemmerArabic.Stemming(item);
                            if (wordStemm.Length>2)
                            hashSet.Add(wordStemm);
                            if (countTerm<hashSet.Count)
                            {
                                countTerm++;
                                newTerm.Terms = wordStemm;
                                newTerm.Docs = " "+ file.File_Name ;
                                db.Term_Document.Add(newTerm);
                                db.SaveChanges();
                            }
                            else
                            {
                                  newTerm = db.Term_Document.Where(f => f.Terms.Equals(wordStemm)).FirstOrDefault();
                                  if (newTerm != null)
                                  {
                                        newTerm.Docs += " "+ file.File_Name ;
                                        db.Entry(newTerm).State = EntityState.Modified;
                                        db.SaveChanges();
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
                            wordStemm = stemmerEnglish.stem(item.ToLower());
                            if (wordStemm.Length > 2)
                                hashSet.Add(wordStemm);
                            if (countTerm < hashSet.Count)
                            {
                                countTerm++;
                                newTerm.Terms = wordStemm.ToLower();
                                newTerm.Docs = " " + file.File_Name ;
                                db.Term_Document.Add(newTerm);
                                db.SaveChanges();
                            }
                            else
                            {
                                newTerm = db.Term_Document.Where(f => f.Terms.Equals(wordStemm.ToLower())).FirstOrDefault();
                                if (newTerm != null)
                                {
                                        newTerm.Docs += " " + file.File_Name ;
                                        db.Entry(newTerm).State = EntityState.Modified;
                                        db.SaveChanges();
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
                    if (!documentCollection.ContainsKey(file))
                    {
                        documentCollection.Add(file, listTermDoc);
                    }
                }
                distinctTerm = db.Term_Document.Select(x => x.Terms).Distinct().ToHashSet<string>();
                termDocumentIncidenceMatrix = BooleanQueryManipulationClass.GetTermDocumentIncidenceMatrix(distinctTerm, documentCollection);
                insert_Distinct_Term_Docs_To_DB(termDocumentIncidenceMatrix);
                ////////  Count Freq Term in Docs;
                string OrderFreqTerm_in_docs = "";
                Files_List = db.Files.ToList();
                foreach (var term in db.Term_Document)
                {
                    foreach (var Name_Doc in Files_List)
                    {
                        OrderFreqTerm_in_docs+= Regex.Matches(term.Docs, " "+Name_Doc.File_Name).Count.ToString()+" ";
                    }
                    term.Freg_Term_in_docs = OrderFreqTerm_in_docs;
                    OrderFreqTerm_in_docs = "";
                }
                db.SaveChanges();
                insertVectorTerm_to_DB(); // for comput Vector Term for All Documents
            }
            return RedirectToAction("Index");
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
        public void insertVectorTerm_to_DB()
        {
            Dictionary<int, double[]> dictionary = new Dictionary<int, double[]>();
            string tempVectorTerm = "";
            int TermID = 0;
            string test = "";
            string[] TempVector;
            double[] TempVectorDo;
            dictionary = VectorSpaceModel.createVector_For_Docs(db.Term_Document.ToList(),db.OrderTerms_DocsBoolean.ToList());
            double[] queryvector = new double[db.OrderTerms_DocsBoolean.ToList().Count];
            foreach (var TermVector in dictionary)
            {
                TempVectorDo = TermVector.Value;
                 TempVector = TempVectorDo.Select(x => x.ToString()).ToArray();
                tempVectorTerm = string.Join(" ", TempVector);
                TermID = TermVector.Key;
                /// insert after recive 
                var distinctTerm = db.OrderTerms_DocsBoolean.Find(TermVector.Key);
                distinctTerm.VectorTerm = tempVectorTerm;
                db.Entry(distinctTerm).State = EntityState.Modified;
                db.SaveChanges();
                //test write to file
                test += TermID + " == " + tempVectorTerm+Environment.NewLine;
            }
              System.IO.File.WriteAllText(@"D:\testVector.txt", test);
        }
        public Dictionary<string, double> VectorSpace_Model(string QueryText)
        {
            Dictionary<string, double> FRVS = VectorSpaceModel.R_Docs_VS(QueryText);
            return FRVS;
        }

        public List<Files> Boolean_Model(string QueryText)
        {
            List<Files> listFileRelevant = new List<Files>();
            string [] listWordQuery = QueryText.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
            bool[] result = new bool[db.Files.Count()];
            string[] tempStr =new string[db.Files.Count()];
            int[] tempInt = new int[db.Files.Count()];
            bool[] tempBool = new bool[db.Files.Count()];
            string wordTemp = "";
            OrderTerms_DocsBoolean term = new OrderTerms_DocsBoolean();
            for (int i = 0; i < listWordQuery.Length; i++)
            {
                if (i == 0)
                {
                    if (listWordQuery[0] == "not")
                    {
                        wordTemp = listWordQuery[1];
                        term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                        if (term != null)
                        {
                            tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            tempInt = Array.ConvertAll(tempStr, int.Parse);
                            result = tempInt.Select(b => b != 1).ToArray();
                        }
                        i++;
                    }
                    else
                    {
                        wordTemp = listWordQuery[0];
                        term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                        if (term != null)
                        {
                            tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            tempInt = Array.ConvertAll(tempStr, int.Parse);
                            result = tempInt.Select(b => b == 1).ToArray();
                        }
                    }
                }
                else
                {
                   
                    if (listWordQuery[i] == "and")
                    {

                        if (listWordQuery[i + 1] == "not")
                        {
                            wordTemp = listWordQuery[i + 2];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                tempBool = tempInt.Select(b => b != 1).ToArray();
                                result = result.Zip(tempBool, (d1, d2) => d1 && d2).ToArray();
                            }
                            i+=2;
                        }
                        else
                        {
                            wordTemp = listWordQuery[i + 1];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                tempBool = tempInt.Select(b => b == 1).ToArray();
                                result = result.Zip(tempBool, (d1, d2) => d1 && d2).ToArray();
                            }
                            i++;

                        }
                    }
                    else if (listWordQuery[i] == "or")
                    {
                        if (listWordQuery[i + 1] == "not")
                        {
                            wordTemp = listWordQuery[i + 2];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                tempBool = tempInt.Select(b => b != 1).ToArray();
                                result = result.Zip(tempBool, (d1, d2) => d1 || d2).ToArray();
                            }
                            i+=2;
                        }
                        else
                        {
                            wordTemp = listWordQuery[i + 1];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                tempBool = tempInt.Select(b => b == 1).ToArray();
                                result = result.Zip(tempBool, (d1, d2) => d1 || d2).ToArray();
                            }
                            i++;
                        }
                    }
                    else if (listWordQuery[i] == "not")
                    {
                        wordTemp = listWordQuery[i + 1];
                        term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                        if (term != null)
                        {
                            tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            tempInt = Array.ConvertAll(tempStr, int.Parse);
                            tempBool = tempInt.Select(b => b != 1).ToArray();
                            result = result.Zip(tempBool, (d1, d2) => d1 && d2).ToArray();
                        }
                        i++;
                    }
                }


                // Arabic Search او و دون

                if (i == 0)
                {
                    if (listWordQuery[0] == "دون")
                    {
                        wordTemp = listWordQuery[1];
                        term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                        if (term != null)
                        {
                            tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            tempInt = Array.ConvertAll(tempStr, int.Parse);
                            result = tempInt.Select(b => b != 1).ToArray();
                        }
                        i++;
                    }
                    else
                    {
                        wordTemp = listWordQuery[0];
                        term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                        if (term != null)
                        {
                            tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            tempInt = Array.ConvertAll(tempStr, int.Parse);
                            result = tempInt.Select(b => b == 1).ToArray();
                        }
                    }
                }
                else
                {

                    if (listWordQuery[i] == "و")
                    {

                        if (listWordQuery[i + 1] == "دون")
                        {
                            wordTemp = listWordQuery[i + 2];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                tempBool = tempInt.Select(b => b != 1).ToArray();
                                result = result.Zip(tempBool, (d1, d2) => d1 && d2).ToArray();
                            }
                            i += 2;
                        }
                        else
                        {
                            wordTemp = listWordQuery[i + 1];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                tempBool = tempInt.Select(b => b == 1).ToArray();
                                result = result.Zip(tempBool, (d1, d2) => d1 && d2).ToArray();
                            }
                            i++;

                        }
                    }
                    else if (listWordQuery[i] == "أو" || listWordQuery[i] == "او")
                    {
                        if (listWordQuery[i + 1] == "دون")
                        {
                            wordTemp = listWordQuery[i + 2];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                tempBool = tempInt.Select(b => b != 1).ToArray();
                                result = result.Zip(tempBool, (d1, d2) => d1 || d2).ToArray();
                            }
                            i += 2;
                        }
                        else
                        {
                            wordTemp = listWordQuery[i + 1];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                tempBool = tempInt.Select(b => b == 1).ToArray();
                                result = result.Zip(tempBool, (d1, d2) => d1 || d2).ToArray();
                            }
                            i++;
                        }
                    }
                    else if (listWordQuery[i] == "دون")
                    {
                        wordTemp = listWordQuery[i + 1];
                        term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                        if (term != null)
                        {
                            tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            tempInt = Array.ConvertAll(tempStr, int.Parse);
                            tempBool = tempInt.Select(b => b != 1).ToArray();
                            result = result.Zip(tempBool, (d1, d2) => d1 && d2).ToArray();
                        }
                        i++;
                    }
                }

            }
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i])
                {
                    listFileRelevant.Add(db.Files.ToList()[i]);
                }
            }
            return listFileRelevant;
        }

        public List<Files> Extended_Boolean_Model(string QueryText)
        {
            Dictionary<int,double> listFileRelevant = new Dictionary<int, double>(); //id files and sim of document
            List<Files> fileR = new List<Files>(); //files list after order by sim
            string[] listWordQuery = QueryText.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
            int indexFile = 0;
            string wordTemp = "";
            string[] tempStr = new string[db.Files.Count()];
            int[] tempInt = new int[db.Files.Count()];
            OrderTerms_DocsBoolean term = new OrderTerms_DocsBoolean();
            double TempSIM_DQ = 0.0;
            foreach (var file in db.Files)
            {
                for (int i = 0; i < listWordQuery.Length; i++)
                {

                    if (i == 0)
                    {
                        if (listWordQuery[i] == "not")
                        {
                            wordTemp = listWordQuery[i + 1];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                TempSIM_DQ = 1 - Math.Pow(tempInt[indexFile], 2);
                            }
                            i++;
                        }
                        else
                        {
                            wordTemp = listWordQuery[i];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                TempSIM_DQ = tempInt[indexFile];
                            }
                            i++;
                        }
                    }
                    else
                    {
                        if (listWordQuery[i] == "not")
                        {
                            wordTemp = listWordQuery[i + 1];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                TempSIM_DQ = 1 - Math.Pow(tempInt[indexFile], 2);
                            }
                            i++;
                        }
                        if (listWordQuery[i] == "and")
                        {
                            wordTemp = listWordQuery[i + 1];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                TempSIM_DQ = 1 - ((double)Math.Sqrt(Math.Pow(1 - TempSIM_DQ, 2) + Math.Pow(1 - tempInt[indexFile], 2) / 2));
                            }
                            i++;
                        }
                        if (listWordQuery[i] == "or")
                        {

                            wordTemp = listWordQuery[i + 1];
                            term = db.OrderTerms_DocsBoolean.Where(t => t.Term.ToLower().Equals(wordTemp)).FirstOrDefault();
                            if (term != null)
                            {
                                tempStr = term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                tempInt = Array.ConvertAll(tempStr, int.Parse);
                                TempSIM_DQ = (double)Math.Sqrt(Math.Pow(1 - TempSIM_DQ, 2) + Math.Pow(1 - tempInt[indexFile], 2)) / 2;
                            }
                            i++;
                        }

                    }

                }

                listFileRelevant.Add(file.File_ID, TempSIM_DQ);
            }

            var sortedDict = from file in listFileRelevant orderby file.Value descending select file;
            foreach (var item in sortedDict)
            {
                if (item.Value > 0)
                {
                    fileR.Add(db.Files.Find(Convert.ToInt16(item.Key)));
                }
            }
            return fileR;
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
     
    }
}