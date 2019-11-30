using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using AIR_SVU_S19.Models;

namespace AIR_SVU_S19
{
    public static class VectorSpaceModel
    {
        static char[] charsSplit = { ' ' };
        static AIR_SVU_S19_Model db = new AIR_SVU_S19_Model();

        public static Dictionary<int, double[]> createVector_For_Docs(List<Term_Document> _FreqTermInDOC, List<OrderTerms_DocsBoolean> _ExistTermInDOC)
        {
            Dictionary<int, double[]> dictionary = new Dictionary<int, double []>();
            int countDocs = _ExistTermInDOC.First().Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries).Length;
            double[] queryvector = new double[countDocs];
            foreach (var term in _ExistTermInDOC)
            {
                var docs= _FreqTermInDOC.Where(t => t.Terms.Equals(term.Term)).FirstOrDefault();
                if (docs == null) continue;
                for (int i = 0; i < countDocs; i++)
                {
                    int Freq = Convert.ToInt16(docs.Freg_Term_in_docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries)[i]);
                    double freqTD = Math.Log(countDocs/Regex.Matches(term.Docs, "1").Count);
                    if (freqTD < 0) freqTD = 0.0;
                    double tfIDF = Freq * freqTD;
                    queryvector[i] = Math.Round(tfIDF,2);
                }
                dictionary.Add(term.ID, queryvector);
                queryvector = new double[countDocs];
            }
            return dictionary;
        }
        public static double[] createVector_For_Query(string Query)
        {
            var listBooleanTerm = db.OrderTerms_DocsBoolean.ToList();
            string poureText = ReturnCleanASCII(Query);
            int count_File = db.Files.Count();
            double[] queryvector = new double[listBooleanTerm.Count];
            poureText = Stopword_Arabic.RemoveStopwords(poureText);
            poureText = Stopword_English.RemoveStopwords(poureText);
            int i = 0;
            foreach (var item in listBooleanTerm)
            {
                    int FTQ = Regex.Matches(poureText, item.Term,RegexOptions.IgnoreCase).Count;
                    double freqTD = Math.Log(count_File / Regex.Matches(item.Docs, "1").Count);
                    if (freqTD < 0) freqTD = 0.0;
                    double tfIDF = FTQ * freqTD;
                    queryvector[i] = Math.Round(tfIDF, 2);
                i++;
            }
            return queryvector;
        }
        public static string ReturnCleanASCII(string s)
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



        public static double dotproduct(double[] v1, double[] v2)
        {
            double product = 0.0;
            if (v1.Length == v2.Length)
            {
                for (int i = 0; i < v1.Length; i++)
                {
                    product += v1[i] * v2[i];
                }
            }
            return product;
        }

        public static double vectorlength(double[] vector)
        {
            double length = 0.0;
            for (int i = 0; i < vector.Length; i++)
            {
                length += Math.Pow(vector[i], 2);
            }

            return Math.Sqrt(length);
        }
        // term frequence in docs
        public static double cosinetheta(double[] v1, double[] v2)
        {
            double lengthV1 = vectorlength(v1);
            double lengthV2 = vectorlength(v2);
            double dotprod = dotproduct(v1, v2);
            return dotprod / (lengthV1 * lengthV2);
        }



        public static Dictionary<string,double> R_Docs_VS(string QueryTXT)
        {
            Dictionary<string, double> listFR = new Dictionary<string, double>();
            double[] QV = createVector_For_Query(QueryTXT);
            int countTermDistinct = db.OrderTerms_DocsBoolean.Count();
            double[] FV = new double[countTermDistinct];
            int indexFile = 0;
            int indexTerm ;
            foreach (var item in db.Files)
            {
                indexTerm = 0;
                foreach (var term in db.OrderTerms_DocsBoolean)
                {
                    FV[indexTerm] =Convert.ToDouble(term.Docs.Split(charsSplit, StringSplitOptions.RemoveEmptyEntries)[indexFile]);
                    indexTerm++;
                } 
                double temp = cosinetheta(QV, FV);
                listFR.Add(item.File_ID.ToString(), temp);
                indexFile++;
            }
            return listFR;
        }

    }
}
