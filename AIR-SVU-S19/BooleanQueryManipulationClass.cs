using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIR_SVU_S19
{
    static class BooleanQueryManipulationClass
    {
        public static Dictionary<string, List<int>> GetTermDocumentIncidenceMatrix(HashSet<string> distinctTerms, Dictionary<string, List<string>> documentCollection)
        {
            Dictionary<string, List<int>> termDocumentIncidenceMatrix = new Dictionary<string, List<int>>();
            List<int> incidenceVector = new List<int>();
            foreach (string term in distinctTerms)
            {
                //incidence vector for each terms
                incidenceVector = new List<int>();
                foreach (KeyValuePair<string, List<string>> p in documentCollection)
                {

                    if (p.Value.Contains(term))
                    {
                        //document contains the term
                        incidenceVector.Add(1);

                    }
                    else
                    {
                        //document do not contains the term
                        incidenceVector.Add(0);
                    }
                }
                termDocumentIncidenceMatrix.Add(term, incidenceVector);

            }
            return termDocumentIncidenceMatrix;
        }

        public class MultiDimDictList : Dictionary<string, List<int>> { }
        public class MultiDimDictStringList : Dictionary<string, List<String>> { }

    }
}