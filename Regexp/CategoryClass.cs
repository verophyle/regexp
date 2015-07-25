// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Verophyle.Regexp
{
    /// <summary>
    /// An input class that matches Unicode categories.
    /// </summary>
    public class CategoryClass : CharClass
    {
        public ISet<UnicodeCategory> Categories { get; private set; }

        static IDictionary<string, UnicodeCategory> cat_map = null;

        protected CategoryClass()
        {
            if (cat_map == null)
                GenCategoryMap();
        }

        public CategoryClass(IEnumerable<char> designation)
            : this()
        {
            var code = new string(designation.ToArray());
            switch (code)
            {
                case "s":
                    Categories = new HashSet<UnicodeCategory>
                    {
                        UnicodeCategory.SpaceSeparator,
                        UnicodeCategory.LineSeparator,
                        UnicodeCategory.ParagraphSeparator
                    };
                    break;
                case "w":
                    Categories = new HashSet<UnicodeCategory>
                    {
                        UnicodeCategory.UppercaseLetter,
                        UnicodeCategory.LowercaseLetter,
                        UnicodeCategory.TitlecaseLetter,
                        UnicodeCategory.ModifierLetter,
                        UnicodeCategory.OtherLetter
                    };
                    break;
                case "d":
                    Categories = new HashSet<UnicodeCategory>
                    {
                        UnicodeCategory.DecimalDigitNumber
                    };
                    break;
                default:
                    var str = new string(designation.ToArray());
                    UnicodeCategory cat;
                    if (cat_map.TryGetValue(str, out cat))
                    {
                        Categories = new HashSet<UnicodeCategory> { cat };
                    }
                    else
                    {
                        throw new Exception("Unknown unicode category " + str);
                    }
                    
                    break;
            }
        }

        public CategoryClass(IEnumerable<UnicodeCategory> categories)
            : this()
        {
            Categories = new HashSet<UnicodeCategory>(categories);
        }

        public CategoryClass(IEnumerable<InputClass<char>> cls)
            : this()
        {
            Inputs.Clear();
            Categories = new HashSet<UnicodeCategory>();

            foreach (var cl in cls)
            {
                Inputs.UnionWith(cl.Inputs);

                CategoryClass cat_cls = cl as CategoryClass;
                if (cat_cls != null)
                    Categories.UnionWith(cat_cls.Categories);
            }
        }

        public override InputClass<char> GetNewClassFromCodes(IEnumerable<int> codes)
        {
            var result = new CategoryClass();
            result.Categories = new HashSet<UnicodeCategory>();
            foreach (var code in codes)
            {
                if (code >= 0)
                    result.Inputs.Add((char)code);
                else
                    result.Categories.Add(GetCodeCat(code));
            }
            return result;
        }

        protected override ISet<int> GenerateInputCodes()
        {
            var inputs = Inputs.Select(ch => (int)ch).ToArray();
            var categories = Categories.Select(GetCatCode).ToArray();
            var combined = inputs.Concat(categories).ToArray();

            return new HashSet<int>(combined);
        }

        static int GetCatCode(UnicodeCategory cat)
        {
            return (-1 * (int)cat) - 1000;
        }

        static UnicodeCategory GetCodeCat(int code)
        {
            return (UnicodeCategory)((code + 1000) * -1);
        }

        public override IInputClassMatcher<char> GetMatcher(IEnumerable<InputClass<char>> input_classes)
        {
            return new CategoryClassMatcher(input_classes);
        }

        string str = null;

        public override string ToString()
        {
#if !DEBUG
            if (str == null)
#endif

            {
                var inputs = Inputs.Select(ch => "" + ch);
                var cats = cat_map.Where(kv => Categories.Contains(kv.Value)).Select(kv => kv.Key);
                var all = inputs.Concat(cats).Select(System.Text.RegularExpressions.Regex.Escape);
                str = string.Format("{0}: {{{1}}}", TransitionIndex, string.Join("|", all));
            }

            return str;
        }

        static void GenCategoryMap()
        {
            cat_map = new Dictionary<string, UnicodeCategory>();

            cat_map.Add("Lu", UnicodeCategory.UppercaseLetter);
            cat_map.Add("Ll", UnicodeCategory.LowercaseLetter);
            cat_map.Add("Lt", UnicodeCategory.TitlecaseLetter);
            cat_map.Add("Lm", UnicodeCategory.ModifierLetter);
            cat_map.Add("Lo", UnicodeCategory.OtherLetter);

            cat_map.Add("Mn", UnicodeCategory.NonSpacingMark);
            cat_map.Add("Mc", UnicodeCategory.SpacingCombiningMark);
            cat_map.Add("Me", UnicodeCategory.EnclosingMark);

            cat_map.Add("Nd", UnicodeCategory.DecimalDigitNumber);
            cat_map.Add("Nl", UnicodeCategory.LetterNumber);
            cat_map.Add("No", UnicodeCategory.OtherNumber);

            cat_map.Add("Zs", UnicodeCategory.SpaceSeparator);
            cat_map.Add("Zl", UnicodeCategory.LineSeparator);
            cat_map.Add("Zp", UnicodeCategory.ParagraphSeparator);

            cat_map.Add("Cc", UnicodeCategory.Control);
            cat_map.Add("Cf", UnicodeCategory.Format);
            cat_map.Add("Cs", UnicodeCategory.Surrogate);
            cat_map.Add("Co", UnicodeCategory.PrivateUse);

            cat_map.Add("Pc", UnicodeCategory.ConnectorPunctuation);
            cat_map.Add("Pd", UnicodeCategory.DashPunctuation);
            cat_map.Add("Ps", UnicodeCategory.OpenPunctuation);
            cat_map.Add("Pe", UnicodeCategory.ClosePunctuation);
            cat_map.Add("Pi", UnicodeCategory.InitialQuotePunctuation);
            cat_map.Add("Pf", UnicodeCategory.FinalQuotePunctuation);
            cat_map.Add("Po", UnicodeCategory.OtherPunctuation);

            cat_map.Add("Sm", UnicodeCategory.MathSymbol);
            cat_map.Add("Sc", UnicodeCategory.CurrencySymbol);
            cat_map.Add("Sk", UnicodeCategory.ModifierSymbol);
            cat_map.Add("So", UnicodeCategory.OtherSymbol);
            cat_map.Add("Cn", UnicodeCategory.OtherNotAssigned);

            foreach (UnicodeCategory cat in Enum.GetValues(typeof(UnicodeCategory)))
            {
                bool found = false;
                foreach (var kv in cat_map)
                {
                    if (kv.Value == cat)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    throw new Exception("Unicode category table is missing " + cat);
            }
        }
    }
}
