using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace VectorSpaceModel
{
    class Program
    {
        static Hashtable DTVector = new Hashtable(); //Hashtable to hold Document Term Vector
        static List<string> wordlist = new List<string>(); //List of terms found in documents
        static Dictionary<double,string> sortedList = new Dictionary<double, string>(); //Documents ranked by VSM with angle value
        static string[] docs ={"gold silver truck", //Query
        "shipment of gold damaged in a fire",//Doc 1
        "delivery of silver arrived in a silver truck", //Doc 2
        "shipment of gold arrived in a truck"}; //Doc 3
        static void Main(string[] args)
        {
            createWordList();
            createVector();
            classify();

            var dict = sortedList;
            foreach (var x in dict.Reverse())
            {
                Console.WriteLine("{0} -> Doc{1}", x.Key, x.Value);
            }
            Console.ReadLine();
        }
        public static void createWordList()
        {
            foreach (string doc in docs)
            {
                wordlist = getWordList(wordlist, doc);
            }
        }
        public static List<string> getWordList(List<string> wordlist, string query)
        {
            Regex exp = new Regex("\\w+", RegexOptions.IgnoreCase);
            MatchCollection MCollection = exp.Matches(query);

            foreach (Match match in MCollection)
            {
                if (!wordlist.Contains(match.Value))
                {
                    wordlist.Add(match.Value);
                }
            }
            return wordlist;
        }
        public static void createVector()
        {
            double[] queryvector;

            for (int j = 0; j < docs.Length; j++)
            {
                queryvector = new double[wordlist.Count];

                for (int i = 0; i < wordlist.Count; i++)
                {

                    double tfIDF = getTF(docs[j], wordlist[i]) * getIDF(wordlist[i]);
                    queryvector[i] = tfIDF;
                }

                if (j == 0) //is it a query?
                {
                    DTVector.Add("Query", queryvector);
                }
                else
                {
                    DTVector.Add(j.ToString(), queryvector);
                }
            }
        }
        public static void classify()
        {
            double temp = 0.0;

            IDictionaryEnumerator _enumerator = DTVector.GetEnumerator();

            double[] queryvector = new double[wordlist.Count];

            Array.Copy((double[])DTVector["Query"], queryvector, wordlist.Count);

            while (_enumerator.MoveNext())
            {
                if (_enumerator.Key.ToString() != "Query")
                {
                    temp = cosinetheta(queryvector, (double[])_enumerator.Value);

                    sortedList.Add(temp, _enumerator.Key.ToString());
                }
            }
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
        private static double getTF(string document, string term)
        {
            string[] queryTerms = Regex.Split(document, "\\s");
            double count = 0;
            foreach (string t in queryTerms)
            {
                if (t == term)
                {
                    count++;
                }
            }
            return count;
        }

        private static double getIDF(string term)
        {
            double df = 0.0;
            //get term frequency of all of the sentences except for the query
            for (int i = 1; i < docs.Length; i++)
            {
                if (docs[i].Contains(term))
                {
                    df++;
                }
            }

            //Get sentence count
            double D = docs.Length - 1; //excluding the query

            double IDF = 0.0;

            if (df > 0)
            {
                IDF = Math.Log(D / df);
            }

            return IDF;
        }

        public static double cosinetheta(double[] v1, double[] v2)
        {
            double lengthV1 = vectorlength(v1);
            double lengthV2 = vectorlength(v2);

            double dotprod = dotproduct(v1, v2);

            return dotprod / (lengthV1 * lengthV2);
        }
    }
}