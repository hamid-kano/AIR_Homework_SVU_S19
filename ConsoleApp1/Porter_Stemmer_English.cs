using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace ConsoleApp1
{
    public class Porter_Stemmer_English 
    {
        String source;
        public string Process(String _source)
        {
            source = _source;
            PerformStep1();
            PerformStep2();
            PerformStep3();
            PerformStep4();
            PerformStep5();
            PerformStep6();
            return source.ToString();

        }

            /* gets rid of plurals and -ed or -ing. e.g.
                   caresses  ->  caress
                   ponies    ->  poni
                   ties      ->  ti
                   caress    ->  caress
                   cats      ->  cat

                   feed      ->  feed
                   agreed    ->  agree
                   disabled  ->  disable

                   matting   ->  mat
                   mating    ->  mate
                   meeting   ->  meet
                   milling   ->  mill
                   messing   ->  mess

                   meetings  ->  meet

            */
            
            public void PerformStep1()
            {
                if (source.EndsWith("s"))
                {
                    if (source.EndsWith("sses") || source.EndsWith("ies"))
                    {
                    source = source.Substring(0, source.Length - 2);
                    }
                    else if (source.Length >= 2 && source.ElementAt(source.Length-2)!= 's')
                    {
                    source = source.Substring(0,source.Length-1);
                    }
                }

                if (source.EndsWith("eed"))
                {
                    var limit = source.Length - 3; // source.Length
                    if (NumberOfConsoantSequences(limit) > 0)
                    {
                    source = source.Substring(0, limit - 1);
                    }
                }
                else
                {
                    var limit = 0;
                    if (source.EndsWith("ed"))
                    {
                        limit = source.Length - 2;
                    }
                    else if (source.EndsWith("ing"))
                    {
                        limit = source.Length - 3;
                    }

                    if (limit != 0 && ContainsVowel(limit))
                    {
                        //source.Size = limit;
                        if (
                            source.EndsWith("at") ||
                            source.EndsWith("bl") ||
                            source.EndsWith("iz")
                        )
                        {
                         source = source.Insert(limit, "e");
                        }
                        else if (EndsWithDoubleConsonant())
                        {
                            var ch = source.Last();
                            if (ch != 'l' && ch != 's' && ch != 'z')
                            {
                               source= source.Substring(0, limit - 1);
                            }
                        }
                        else if (
                            NumberOfConsoantSequences(limit - 1) == 1 &&
                            HasCvcAt(source.Length - 1)
                        )
                        {
                        source = source.Insert(limit, "e");
                    }
                    }
                }
            }

            /* turns terminal y to i when there is another vowel in the stem. */
            public void PerformStep2()
            {
                if (source.EndsWith("y")
                    && ContainsVowel(source.Length - 2)
                )
                {
                source = source.Remove(source.Length - 1);
                source.Insert(source.Length, "i");
                }
            }

            /* maps double suffices to single ones. so -ization ( = -ize plus
               -ation) maps to -ize etc. note that the string before the suffix must give
               m() > 0. */
            public void PerformStep3()
            {
                if (source.Length < 2)
                    return;

                switch (source.ElementAt(source.Length-2))
                {
                    case 'a':
                    if (source.EndsWith("ational")) { source = source.Replace("ational", "ate"); break; }
                        source = source.Replace("tional", "tion");
                        break;
                    case 'c':
                    if (source.EndsWith("enci")) { source = source.Replace("enci", "ence"); break; }
                        source = source.Replace("anci", "ance");
                        break;
                    case 'e':
                        source = source.Replace("izer", "ize");
                        break;
                    case 'l':
                    if (source.EndsWith("bli")) { source = source.Replace("bli", "ble"); break; }
                    if (source.EndsWith("alli")) { source = source.Replace("alli", "al"); break; }
                    if (source.EndsWith("entli")) { source = source.Replace("entli", "ent"); break; }
                    if (source.EndsWith("eli")) { source = source.Replace("eli", "e"); break; }
                        source = source.Replace("ousli", "ous");
                        break;
                    case 'o':
                    if (source.EndsWith("ization")) { source = source.Replace("ization", "ize"); break; }
                    if (source.EndsWith("ation")) { source = source.Replace("ation", "ate"); break; }
                        source = source.Replace("ator", "ate");
                        break;
                    case 's':
                    if (source.EndsWith("alism")) { source = source.Replace("alism", "al"); break; }
                    if (source.EndsWith("iveness")) { source = source.Replace("iveness", "ive"); break; }
                    if (source.EndsWith("fulness")) { source = source.Replace("fulness", "ful"); break; }
                        source = source.Replace("ousness", "ous");
                        break;
                    case 't':
                    if (source.EndsWith("aliti")) { source = source.Replace("aliti", "al"); break; }
                    if (source.EndsWith("iviti")) { source = source.Replace("iviti", "ive"); break; }
                        source = source.Replace("biliti", "ble");
                        break;
                    case 'g':
                        source = source.Replace("logi", "log");
                        break;
                }
            }


            /* deals with -ic-, -full, -ness etc. similar strategy to step3. */
            public void PerformStep4()
            {
                if (source.Length == 0)
                    return;

                switch (source.Last())
                {
                    case 'e':
                    if (source.EndsWith("icate")) { source = source.Replace("icate", "ic"); break; }
                    if (source.EndsWith("ative")) { source = source.Replace("ative", ""); break; }
                        source = source.Replace("alize", "al");
                        break;
                    case 'i':
                        source = source.Replace("iciti", "ic");
                        break;
                    case 'l':
                    if (source.EndsWith("ical")) { source = source.Replace("ical", "ic"); break; }
                        source = source.Replace("ful","");
                        break;
                    case 's':
                        source = source.Replace("ness","");
                        break;
                }
            }

            public void PerformStep5()
            {
                if (source.Length < 2)
                    return;

                switch (source.ElementAt(source.Length - 2))
                {
                    case 'a':
                        source = source.Replace("al","");
                        return;
                    case 'c':
                    if (source.EndsWith("ance")) { source = source.Replace("ance", ""); return; }
                        source = source.Replace("ence","");
                        return;
                    case 'e':
                        source = source.Replace("er","");
                        return;
                    case 'i':
                        source = source.Replace("ic","");
                        return;
                    case 'l':
                    if (source.EndsWith("able")) { source = source.Replace("able", ""); return; }
                        source = source.Replace("ible","");
                        return;
                    case 'n':
                    if (source.EndsWith("ant")) { source = source.Replace("ant", ""); return; }
                    if (source.EndsWith("ement")) { source = source.Replace("ement", ""); return; }
                    if (source.EndsWith("ment")) { source = source.Replace("ment", ""); return; }
                        source = source.Replace("ent","");
                        return;
                    case 'o':
                    if (source.EndsWith("tion")) { source = source.Replace("tion", "t"); return; }
                    if (source.EndsWith("sion")) { source = source.Replace("sion", "s"); return; }
                        source = source.Replace("ou","");
                        return;
                    case 's':
                        source = source.Replace("ism","");
                        return;
                    case 't':
                    if (source.EndsWith("ate")) { source = source.Replace("ate", ""); return; }
                        source = source.Replace("iti","");
                        return;
                    case 'u':
                        source = source.Replace("ous","");
                        return;
                    case 'v':
                        source = source.Replace("ive","");
                        return;
                    case 'z':
                        source = source.Replace("ize","");
                        return;
                    default:
                        return;
                }
            }

            public void PerformStep6()
            {
                switch (source.Last())
                {
                    case 'e':
                        var a = NumberOfConsoantSequences(source.Length - 1);
                    if (a > 1 || a == 1 && !HasCvcAt(source.Length - 2))
                        source = source.Substring(0,source.Length-1); 
                        break;
                    case 'l' when ContainsDoubleConsonantAt(source.Length - 1) && NumberOfConsoantSequences(source.Length - 2) > 0:
                    source = source.Substring(0, source.Length - 1);
                    break;
                }
            }


        public int NumberOfConsoantSequences(int limit)
        {
            var result = 0;
            var i = 0;

            while (true)
            {
                if (i > limit) return result;
                if (!IsConsonant(i)) break;
                i++;
            }
            i++;

            while (true)
            {
                while (true)
                {
                    if (i > limit) return result;
                    if (IsConsonant(i)) break;
                    i++;
                }
                i++;
                result++;
                while (true)
                {
                    if (i > limit) return result;
                    if (!IsConsonant(i)) break;
                    i++;
                }
                i++;
            }
        }

        public bool ContainsVowel(int limit)
        {
            for (var i = 0; i <= limit; i++)
                if (!IsConsonant(i))
                    return true;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsConsonant(int index)
        {
            switch (source.ElementAt(index))
            {
                case 'a': case 'e': case 'i': case 'o': case 'u': return false;
                case 'y': return (index == 0) || !IsConsonant(index - 1);
                default: return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsVowel(int index)
        {
            switch (source.ElementAt(index))
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    return true;
                default:
                    return false;
            }
        }

        public bool ContainsDoubleConsonantAt(int index)
        {
            if (index < 1) return false;
            return source.ElementAt(index) == source.ElementAt(index-1)&& IsConsonant(index);
        }

        public bool EndsWithDoubleConsonant() =>
            ContainsDoubleConsonantAt(source.Length - 1);


        /* HasCvcAt(i) is true <=> i-2,i-1,i has the form consonant - vowel - consonant
		   and also if the second c is not w,x or y. this is used when trying to
		   restore an e at the end of a short word. e.g.

			  cav(e), lov(e), hop(e), crim(e), but
			  snow, box, tray.
		*/
        public bool HasCvcAt(int index)
        {
            if (index < 2 || !IsConsonant(index) || IsConsonant(index - 1) || !IsConsonant(index - 2))
                return false;

            int ch =source.IndexOf(source.Last());
            return ch != 'w' && ch != 'x' && ch != 'y';
        }

    }
}


