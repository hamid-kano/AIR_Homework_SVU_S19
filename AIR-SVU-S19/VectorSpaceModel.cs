﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AIR_SVU_S19
{
    public static class VectorSpaceModel
    {
        static Hashtable DTVector = new Hashtable(); //Hashtable to hold Document Term Vector
        static List<string> wordlist = new List<string>(); //Terms From  OrderTerms_DocsBoolean's table in dataBase
        static string[] docs; // contents of docs;
        static string QueryText;
        public static Hashtable Preparation_Vectors_Dcos(List<string> _wordlist , string[] _docs)
        {
            wordlist = _wordlist;
            docs = _docs;
            return  createVector_For_Docs();
        }
        // use when incoming query;
        public static void Add_Query_to_WordList()
        {
                wordlist = getWordList(wordlist, docs[0]);
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



        public static Hashtable createVector_For_Docs()
        {
            double[] queryvector;
            for (int j = 0; j < docs.Length; j++)
            {
                queryvector = new double[wordlist.Count];
                for (int i = 0; i < wordlist.Count; i++)
                {
                    double tfIDF = getTF(docs[j], wordlist[i]) * getIDF(wordlist[i]);
                    queryvector[i] = tfIDF;
                    DTVector.Add(j.ToString(), queryvector);
                }
            }
            return DTVector;
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
            for (int i = 0; i < docs.Length; i++)
            {
                if (docs[i].Contains(term))
                {
                    df++;
                }
            }

            //Get sentence count
            double D = docs.Length;

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
