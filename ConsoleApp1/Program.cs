using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static string[] _delimiters = new string[]
              {"\n"};
        static void Main(string[] args)
        {
            //string input = File.ReadAllText(@"D:\list.txt");
            // //string[] lines = input.Split(new[] { "\r\n", "\r", "\n" },StringSplitOptions.None);

            // //var filestream = new System.IO.FileStream(@"D:\list.txt",
            // //                              System.IO.FileMode.Open,
            // //                              System.IO.FileAccess.Read,
            // //                              System.IO.FileShare.ReadWrite);
            // //var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
            // ////string lineOfText = "";
            // ////// Create a file to write to.
            // ////string createText = "";
           // string readText = File.ReadAllText(@"D:\list.txt");
            StreamReader file =new StreamReader(@"D:\list.txt");
            // string replacement = Regex.Replace(input, @"\t|\n|\r", " ");
            // //foreach (var term in lines)
            // //{              
            // //    //Do something with the lineOfText
            // //    outPut_text += "{"+'"'+ term +'"'+", true},"+ Environment.NewLine;
            // //}
            // File.WriteAllText(@"D:\replacement.txt", replacement);
            //TextReader ss = new StringReader("computer");
            ////TextReader ss  = new StreamReader(@"D:\replacement.txt");
            //Porter_Stemmer_English por = new Porter_Stemmer_English();
            //string  input= Console.ReadLine();
            //Porter2 po = new Porter2();

            //while (input != "0")
            //{
            //    Console.WriteLine(po.stem(input));
            //    input= Console.ReadLine();
            //}
            string outputTXT="";
            string line;
            int i;
            Dictionary<string, bool> _stops = new Dictionary<string, bool>();
               while ((line = file.ReadLine()) != null)
                    {
                try
                {
                     _stops.Add(line, true);
                }
                catch (Exception)
                {
                    //  throw;
                    Console.WriteLine("----- " + line + " -------");
                }
  
               }

            foreach (var pair in _stops)
                    {
                      outputTXT += "{" + pair.Key + "," + pair.Value + "}," + Environment.NewLine;
                // Console.WriteLine("FOREACH KEYVALUEPAIR: {0}, {1}", pair.Key, pair.Value);
            }

            File.WriteAllText(@"D:\replacement.txt", outputTXT);
            //  Console.WriteLine(ss.ReadToEnd().ToString());
            Console.ReadLine();

        }
    }
}
