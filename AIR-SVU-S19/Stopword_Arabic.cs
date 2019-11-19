﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AIR_SVU_S19
{
    static class Stopword_Arabic
    {
        /// <summary>
        /// Tool to remove unwanted words such as 'آل' or 'آ'.
        /// </summary>

        /// <summary>
        /// Words we want to remove.
        /// </summary>
        static Dictionary<string, bool> _stops = new Dictionary<string, bool>
        {
            {"،", true},
            {"ء", true},
            {"ءَ", true},
            {"آ", true},
            {"آب", true},
            {"آذار", true},
            {"آض", true},
            {"آل", true},
            {"آمينَ", true},
            {"آناء", true},
            {"آنفا", true},
            {"آه", true},
            {"آهاً", true},
            {"آهٍ", true},
            {"آهِ", true},
            {"أ", true},
            {"أبدا", true},
            {"أبريل", true},
            {"أبو", true},
            {"أبٌ", true},
            {"أجل", true},
            {"أجمع", true},
            {"أحد", true},
            {"أخبر", true},
            {"أخذ", true},
            {"أخو", true},
            {"أخٌ", true},
            {"أربع", true},
            {"أربعاء", true},
            {"أربعة", true},
            {"أربعمئة", true},
            {"أربعمائة", true},
            {"أرى", true},
            {"أسكن", true},
            {"أصبح", true},
            {"أصلا", true},
            {"أضحى", true},
            {"أطعم", true},
            {"أعطى", true},
            {"أعلم", true},
            {"أغسطس", true},
            {"أفريل", true},
            {"أفعل به", true},
            {"أفٍّ", true},
            {"أقبل", true},
            {"أكتوبر", true},
            {"أل", true},
            {"ألا", true},
            {"ألف", true},
            {"ألفى", true},
            {"أم", true},
            {"أما", true},
            {"أمام", true},
            {"أمامك", true},
            {"أمامكَ", true},
            {"أمد", true},
            {"أمس", true},
            {"أمسى", true},
            {"أمّا", true},
            {"أن", true},
            {"أنا", true},
            {"أنبأ", true},
            {"أنت", true},
            {"أنتم", true},
            {"أنتما", true},
            {"أنتن", true},
            {"أنتِ", true},
            {"أنشأ", true},
            {"أنه", true},
            {"أنًّ", true},
            {"أنّى", true},
            {"أهلا", true},
            {"أو", true},
            {"أوت", true},
            {"أوشك", true},
            {"أول", true},
            {"أولئك", true},
            {"أولاء", true},
            {"أولالك", true},
            {"أوّهْ", true},
            {"أى", true},
            {"أي", true},
            {"أيا", true},
            {"أيار", true},
            {"أيضا", true},
            {"أيلول", true},
            {"أين", true},
            {"أيّ", true},
            {"أيّان", true},
            {"أُفٍّ", true},
            {"ؤ", true},
            {"إحدى", true},
            {"إذ", true},
            {"إذا", true},
            {"إذاً", true},
            {"إذما", true},
            {"إذن", true},
            {"إزاء", true},
            {"إلى", true},
            {"إلي", true},
            {"إليكم", true},
            {"إليكما", true},
            {"إليكنّ", true},
            {"إليكَ", true},
            {"إلَيْكَ", true},
            {"إلّا", true},
            {"إمّا", true},
            {"إن", true},
            {"إنَّ", true},
            {"إى", true},
            {"إياك", true},
            {"إياكم", true},
            {"إياكما", true},
            {"إياكن", true},
            {"إيانا", true},
            {"إياه", true},
            {"إياها", true},
            {"إياهم", true},
            {"إياهما", true},
            {"إياهن", true},
            {"إياي", true},
            {"إيهٍ", true},
            {"ئ", true},
            {"ا", true},
            {"ا?", true},
            {"ا?ى", true},
            {"االا", true},
            {"االتى", true},
            {"ابتدأ", true},
            {"ابين", true},
            {"اتخذ", true},
            {"اثر", true},
            {"اثنا", true},
            {"اثنان", true},
            {"اثني", true},
            {"اثنين", true},
            {"اجل", true},
            {"احد", true},
            {"اخرى", true},
            {"اخلولق", true},
            {"اذا", true},
            {"اربعة", true},
            {"اربعون", true},
            {"اربعين", true},
            {"ارتدّ", true},
            {"استحال", true},
            {"اصبح", true},
            {"اضحى", true},
            {"اطار", true},
            {"اعادة", true},
            {"اعلنت", true},
            {"اف", true},
            {"اكثر", true},
            {"اكد", true},
            {"الآن", true},
            {"الألاء", true},
            {"الألى", true},
            {"الا", true},
            {"الاخيرة", true},
            {"الان", true},
            {"الاول", true},
            {"الاولى", true},
            {"التى", true},
            {"التي", true},
            {"الثاني", true},
            {"الثانية", true},
            {"الحالي", true},
            {"الذاتي", true},
            {"الذى", true},
            {"الذي", true},
            {"الذين", true},
            {"السابق", true},
            {"الف", true},
            {"اللاتي", true},
            {"اللتان", true},
            {"اللتيا", true},
            {"اللتين", true},
            {"اللذان", true},
            {"اللذين", true},
            {"اللواتي", true},
            {"الماضي", true},
            {"المقبل", true},
            {"الوقت", true},
            {"الى", true},
            {"الي", true},
            {"اليه", true},
            {"اليها", true},
            {"اليوم", true},
            {"اما", true},
            {"امام", true},
            {"امس", true},
            {"امسى", true},
            {"ان", true},
            {"انبرى", true},
            {"انقلب", true},
            {"انه", true},
            {"انها", true},
            {"او", true},
            {"اول", true},
            {"اي", true},
            {"ايار", true},
            {"ايام", true},
            {"ايضا", true},
            {"ب", true},
            {"بؤسا", true},
            {"بإن", true},
            {"بئس", true},
            {"باء", true},
            {"بات", true},
            {"باسم", true},
            {"بان", true},
            {"بخٍ", true},
            {"بد", true},
            {"بدلا", true},
            {"برس", true},
            {"بسبب", true},
            {"بسّ", true},
            {"بشكل", true},
            {"بضع", true},
            {"بطآن", true},
            {"بعد", true},
            {"بعدا", true},
            {"بعض", true},
            {"بغتة", true},
            {"بل", true},
            {"بلى", true},
            {"بن", true},
            {"به", true},
            {"بها", true},
            {"بهذا", true},
            {"بيد", true},
            {"بين", true},
            {"بَسْ", true},
            {"بَلْهَ", true},
            {"ة", true},
            {"ت", true},
            {"تاء", true},
            {"تارة", true},
            {"تاسع", true},
            {"تانِ", true},
            {"تانِك", true},
            {"تبدّل", true},
            {"تجاه", true},
            {"تحت", true},
            {"تحوّل", true},
            {"تخذ", true},
            {"ترك", true},
            {"تسع", true},
            {"تسعة", true},
            {"تسعمئة", true},
            {"تسعمائة", true},
            {"تسعون", true},
            {"تسعين", true},
            {"تشرين", true},
            {"تعسا", true},
            {"تعلَّم", true},
            {"تفعلان", true},
            {"تفعلون", true},
            {"تفعلين", true},
            {"تكون", true},
            {"تلقاء", true},
            {"تلك", true},
            {"تم", true},
            {"تموز", true},
            {"تينك", true},
            {"تَيْنِ", true},
            {"تِه", true},
            {"تِي", true},
            {"ث", true},
            {"ثاء", true},
            {"ثالث", true},
            {"ثامن", true},
            {"ثان", true},
            {"ثاني", true},
            {"ثلاث", true},
            {"ثلاثاء", true},
            {"ثلاثة", true},
            {"ثلاثمئة", true},
            {"ثلاثمائة", true},
            {"ثلاثون", true},
            {"ثلاثين", true},
            {"ثم", true},
            {"ثمان", true},
            {"ثمانمئة", true},
            {"ثمانون", true},
            {"ثماني", true},
            {"ثمانية", true},
            {"ثمانين", true},
            {"ثمنمئة", true},
            {"ثمَّ", true},
            {"ثمّ", true},
            {"ثمّة", true},
            {"ج", true},
            {"جانفي", true},
            {"جدا", true},
            {"جعل", true},
            {"جلل", true},
            {"جمعة", true},
            {"جميع", true},
            {"جنيه", true},
            {"جوان", true},
            {"جويلية", true},
            {"جير", true},
            {"جيم", true},
            {"ح", true},
            {"حاء", true},
            {"حادي", true},
            {"حار", true},
            {"حاشا", true},
            {"حاليا", true},
            {"حاي", true},
            {"حبذا", true},
            {"حبيب", true},
            {"حتى", true},
            {"حجا", true},
            {"حدَث", true},
            {"حرى", true},
            {"حزيران", true},
            {"حسب", true},
            {"حقا", true},
            {"حمدا", true},
            {"حمو", true},
            {"حمٌ", true},
            {"حوالى", true},
            {"حول", true},
            {"حيث", true},
            {"حيثما", true},
            {"حين", true},
            {"حيَّ", true},
            {"حَذارِ", true},
            {"خ", true},
            {"خاء", true},
            {"خاصة", true},
            {"خال", true},
            {"خامس", true},
            {"خبَّر", true},
            {"خلا", true},
            {"خلافا", true},
            {"خلال", true},
            {"خلف", true},
            {"خمس", true},
            {"خمسة", true},
            {"خمسمئة", true},
            {"خمسمائة", true},
            {"خمسون", true},
            {"خمسين", true},
            {"خميس", true},
            {"د", true},
            {"دال", true},
            {"درهم", true},
            {"درى", true},
            {"دواليك", true},
            {"دولار", true},
            {"دون", true},
            {"دونك", true},
            {"ديسمبر", true},
            {"دينار", true},
            {"ذ", true},
            {"ذا", true},
            {"ذات", true},
            {"ذاك", true},
            {"ذال", true},
            {"ذانك", true},
            {"ذانِ", true},
            {"ذلك", true},
            {"ذهب", true},
            {"ذو", true},
            {"ذيت", true},
            {"ذينك", true},
            {"ذَيْنِ", true},
            {"ذِه", true},
            {"ذِي", true},
            {"ر", true},
            {"رأى", true},
            {"راء", true},
            {"رابع", true},
            {"راح", true},
            {"رجع", true},
            {"رزق", true},
            {"رويدك", true},
            {"ريال", true},
            {"ريث", true},
            {"رُبَّ", true},
            {"ز", true},
            {"زاي", true},
            {"زعم", true},
            {"زود", true},
            {"زيارة", true},
            {"س", true},
            {"ساء", true},
            {"سابع", true},
            {"سادس", true},
            {"سبت", true},
            {"سبتمبر", true},
            {"سبحان", true},
            {"سبع", true},
            {"سبعة", true},
            {"سبعمئة", true},
            {"سبعمائة", true},
            {"سبعون", true},
            {"سبعين", true},
            {"ست", true},
            {"ستة", true},
            {"ستكون", true},
            {"ستمئة", true},
            {"ستمائة", true},
            {"ستون", true},
            {"ستين", true},
            {"سحقا", true},
            {"سرا", true},
            {"سرعان", true},
            {"سقى", true},
            {"سمعا", true},
            {"سنة", true},
            {"سنتيم", true},
            {"سنوات", true},
            {"سوف", true},
            {"سوى", true},
            {"سين", true},
            {"ش", true},
            {"شباط", true},
            {"شبه", true},
            {"شتانَ", true},
            {"شخصا", true},
            {"شرع", true},
            {"شمال", true},
            {"شيكل", true},
            {"شين", true},
            {"شَتَّانَ", true},
            {"ص", true},
            {"صاد", true},
            {"صار", true},
            {"صباح", true},
            {"صبر", true},
            {"صبرا", true},
            {"صدقا", true},
            {"صراحة", true},
            {"صفر", true},
            {"صهٍ", true},
            {"صهْ", true},
            {"ض", true},
            {"ضاد", true},
            {"ضحوة", true},
            {"ضد", true},
            {"ضمن", true},
            {"ط", true},
            {"طاء", true},
            {"طاق", true},
            {"طالما", true},
            {"طرا", true},
            {"طفق", true},
            {"طَق", true},
            {"ظ", true},
            {"ظاء", true},
            {"ظل", true},
            {"ظلّ", true},
            {"ظنَّ", true},
            {"ع", true},
            {"عاد", true},
            {"عاشر", true},
            {"عام", true},
            {"عاما", true},
            {"عامة", true},
            {"عجبا", true},
            {"عدا", true},
            {"عدة", true},
            {"عدد", true},
            {"عدم", true},
            {"عدَّ", true},
            {"عسى", true},
            {"عشر", true},
            {"عشرة", true},
            {"عشرون", true},
            {"عشرين", true},
            {"عل", true},
            {"علق", true},
            {"علم", true},
            {"على", true},
            {"علي", true},
            {"عليك", true},
            {"عليه", true},
            {"عليها", true},
            {"علًّ", true},
            {"عن", true},
            {"عند", true},
            {"عندما", true},
            {"عنه", true},
            {"عنها", true},
            {"عوض", true},
            {"عيانا", true},
            {"عين", true},
            {"عَدَسْ", true},
            {"غ", true},
            {"غادر", true},
            {"غالبا", true},
            {"غدا", true},
            {"غداة", true},
            {"غير", true},
            {"غين", true},
            {"ـ", true},
            {"ف", true},
            {"فإن", true},
            {"فاء", true},
            {"فان", true},
            {"فانه", true},
            {"فبراير", true},
            {"فرادى", true},
            {"فضلا", true},
            {"فقد", true},
            {"فقط", true},
            {"فكان", true},
            {"فلان", true},
            {"فلس", true},
            {"فهو", true},
            {"فو", true},
            {"فوق", true},
            {"فى", true},
            {"في", true},
            {"فيفري", true},
            {"فيه", true},
            {"فيها", true},
            {"ق", true},
            {"قاطبة", true},
            {"قاف", true},
            {"قال", true},
            {"قام", true},
            {"قبل", true},
            {"قد", true},
            {"قرش", true},
            {"قطّ", true},
            {"قلما", true},
            {"قوة", true},
            {"ك", true},
            {"كأن", true},
            {"كأنّ", true},
            {"كأيّ", true},
            {"كأيّن", true},
            {"كاد", true},
            {"كاف", true},
            {"كان", true},
            {"كانت", true},
            {"كانون", true},
            {"كثيرا", true},
            {"كذا", true},
            {"كذلك", true},
            {"كرب", true},
            {"كسا", true},
            {"كل", true},
            {"كلتا", true},
            {"كلم", true},
            {"كلَّا", true},
            {"كلّما", true},
            {"كم", true},
            {"كما", true},
            {"كن", true},
            {"كى", true},
            {"كيت", true},
            {"كيف", true},
            {"كيفما", true},
            {"كِخ", true},
            {"ل", true},
            {"لأن", true},
            {"لا", true},
            {"لا سيما", true},
            {"لات", true},
            {"لازال", true},
            {"لاسيما", true},
            {"لام", true},
            {"لايزال", true},
            {"لبيك", true},
            {"لدن", true},
            {"لدى", true},
            {"لدي", true},
            {"لذلك", true},
            {"لعل", true},
            {"لعلَّ", true},
            {"لعمر", true},
            {"لقاء", true},
            {"لكن", true},
            {"لكنه", true},
            {"لكنَّ", true},
            {"للامم", true},
            {"لم", true},
            {"لما", true},
            {"لمّا", true},
            {"لن", true},
            {"له", true},
            {"لها", true},
            {"لهذا", true},
            {"لهم", true},
            {"لو", true},
            {"لوكالة", true},
            {"لولا", true},
            {"لوما", true},
            {"ليت", true},
            {"ليرة", true},
            {"ليس", true},
            {"ليسب", true},
            {"م", true},
            {"مئة", true},
            {"مئتان", true},
            {"ما", true},
            {"ما أفعله", true},
            {"ما انفك", true},
            {"ما برح", true},
            {"مائة", true},
            {"ماانفك", true},
            {"مابرح", true},
            {"مادام", true},
            {"ماذا", true},
            {"مارس", true},
            {"مازال", true},
            {"مافتئ", true},
            {"ماي", true},
            {"مايزال", true},
            {"مايو", true},
            {"متى", true},
            {"مثل", true},
            {"مذ", true},
            {"مرّة", true},
            {"مساء", true},
            {"مع", true},
            {"معاذ", true},
            {"معه", true},
            {"مقابل", true},
            {"مكانكم", true},
            {"مكانكما", true},
            {"مكانكنّ", true},
            {"مكانَك", true},
            {"مليار", true},
            {"مليم", true},
            {"مليون", true},
            {"مما", true},
            {"من", true},
            {"منذ", true},
            {"منه", true},
            {"منها", true},
            {"مه", true},
            {"مهما", true},
            {"ميم", true},
            {"ن", true},
            {"نا", true},
            {"نبَّا", true},
            {"نحن", true},
            {"نحو", true},
            {"نعم", true},
            {"نفس", true},
            {"نفسه", true},
            {"نهاية", true},
            {"نوفمبر", true},
            {"نون", true},
            {"نيسان", true},
            {"نيف", true},
            {"نَخْ", true},
            {"نَّ", true},
            {"ه", true},
            {"هؤلاء", true},
            {"ها", true},
            {"هاء", true},
            {"هاكَ", true},
            {"هبّ", true},
            {"هذا", true},
            {"هذه", true},
            {"هل", true},
            {"هللة", true},
            {"هلم", true},
            {"هلّا", true},
            {"هم", true},
            {"هما", true},
            {"همزة", true},
            {"هن", true},
            {"هنا", true},
            {"هناك", true},
            {"هنالك", true},
            {"هو", true},
            {"هي", true},
            {"هيا", true},
            {"هيهات", true},
            {"هيّا", true},
            {"هَؤلاء", true},
            {"هَاتانِ", true},
            {"هَاتَيْنِ", true},
            {"هَاتِه", true},
            {"هَاتِي", true},
            {"هَجْ", true},
            {"هَذا", true},
            {"هَذانِ", true},
            {"هَذَيْنِ", true},
            {"هَذِه", true},
            {"هَذِي", true},
            {"هَيْهات", true},
            {"و", true},
            {"و6", true},
            {"وأبو", true},
            {"وأن", true},
            {"وا", true},
            {"واحد", true},
            {"واضاف", true},
            {"واضافت", true},
            {"واكد", true},
            {"والتي", true},
            {"والذي", true},
            {"وان", true},
            {"واهاً", true},
            {"واو", true},
            {"واوضح", true},
            {"وبين", true},
            {"وثي", true},
            {"وجد", true},
            {"وراءَك", true},
            {"ورد", true},
            {"وعلى", true},
            {"وفي", true},
            {"وقال", true},
            {"وقالت", true},
            {"وقد", true},
            {"وقف", true},
            {"وكان", true},
            {"وكانت", true},
            {"ولا", true},
            {"ولايزال", true},
            {"ولكن", true},
            {"ولم", true},
            {"وله", true},
            {"وليس", true},
            {"ومع", true},
            {"ومن", true},
            {"وهب", true},
            {"وهذا", true},
            {"وهو", true},
            {"وهي", true},
            {"وَيْ", true},
            {"وُشْكَانَ", true},
            {"ى", true},
            {"ي", true},
            {"ياء", true},
            {"يفعلان", true},
            {"يفعلون", true},
            {"يكون", true},
            {"يلي", true},
            {"يمكن", true},
            {"يمين", true},
            {"ين", true},
            {"يناير", true},
            {"يوان", true},
            {"يورو", true},
            {"يوليو", true},
            {"يوم", true},
            {"يونيو", true},
            {"أيّان", true}
        };

        /// <summary>
        /// Chars that separate words.
        /// </summary>
        static char[] _delimiters = new char[]
            {
            ' ',
            ',',
            ';',
            '.'
            };

        /// <summary>
        /// Remove stopwords from string.
        /// </summary>
        public static string RemoveStopwords(string input)
        {
            string replacement = Regex.Replace(input, @"\t|\n|\r", " ");
            // 1
            // Split parameter into words
            var words = replacement.Split(_delimiters,
                StringSplitOptions.RemoveEmptyEntries);
            // 2
            // Allocate new dictionary to store found words
            var found = new Dictionary<string, bool>();
            // 3
            // Store results in this StringBuilder
            StringBuilder builder = new StringBuilder();
            // 4
            // Loop through all words
            foreach (string currentWord in words)
            {
                // 5
                // Convert to lowercase
                string lowerWord = currentWord.ToLower();
                // 6
                // If this is a usable word, add it
                if (!_stops.ContainsKey(lowerWord) &&
                    !found.ContainsKey(lowerWord))
                {
                    builder.Append(currentWord).Append(' ');
                    found.Add(lowerWord, true);
                }
            }
            // 7
            // Return string with words removed
            return builder.ToString().Trim();
        }
    }
}