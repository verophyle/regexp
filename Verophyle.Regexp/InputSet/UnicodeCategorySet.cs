// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Verophyle.Regexp.InputSet
{
    /// <summary>
    /// An input class that matches Unicode categories.
    /// </summary>
    public class UnicodeCategorySet : CharSet
    {
        static readonly IDictionary<string, UnicodeCategory> catsByName;

        ISet<UnicodeCategory> categories;

        static UnicodeCategorySet()
        {
            catsByName = GenCategoryMap();
        }

        internal UnicodeCategorySet()
            : base()
        {
        }

        public UnicodeCategorySet(IEnumerable<char> designation)
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
                    if (catsByName.TryGetValue(str, out cat))
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

        public UnicodeCategorySet(IEnumerable<UnicodeCategory> categories)
            : this()
        {
            Categories = new HashSet<UnicodeCategory>(categories);
        }

        public UnicodeCategorySet(IEnumerable<IInputSet<char>> cls)
            : this()
        {
            Inputs.Clear();
            Categories = new HashSet<UnicodeCategory>();

            foreach (var cl in cls)
            {
                Inputs.UnionWith(cl.Inputs);

                var ucs = cl as UnicodeCategorySet;
                if (ucs != null)
                    Categories.UnionWith(ucs.Categories);
            }
        }

        public ISet<UnicodeCategory> Categories
        {
            get { return categories ?? (categories = new HashSet<UnicodeCategory>()); }
            private set { categories = value; }
        }

        public override ISet<int> InputCodes
        {
            get { return inputCodes ?? (inputCodes = GenerateInputCodes()); }
            internal set { inputCodes = value; }
        }

        internal static int GetCatCode(UnicodeCategory cat)
        {
            return (-1 * (int)cat) - 1000;
        }

        internal static UnicodeCategory GetCodeCat(int code)
        {
            return (UnicodeCategory)((code + 1000) * -1);
        }

        ISet<int> GenerateInputCodes()
        {
            var inputs = Inputs.Select(ch => (int)ch).ToArray();
            var categories = Categories.Select(GetCatCode).ToArray();
            var combined = inputs.Concat(categories).ToArray();

            return new HashSet<int>(combined);
        }

        string str = null;

        public override string ToString()
        {
#if !DEBUG
            if (str == null)
#endif

            {
                var inputs = Inputs.Select(ch => "" + ch);
                var cats = catsByName.Where(kv => Categories.Contains(kv.Value)).Select(kv => kv.Key);
                var all = inputs.Concat(cats).Select(System.Text.RegularExpressions.Regex.Escape);
                str = string.Format("{0}: {{{1}}}", TransitionIndex, string.Join("|", all));
            }

            return str;
        }

        static IDictionary<string, UnicodeCategory> GenCategoryMap()
        {
            var catMap = new Dictionary<string, UnicodeCategory>();

            catMap.Add("Lu", UnicodeCategory.UppercaseLetter);
            catMap.Add("Ll", UnicodeCategory.LowercaseLetter);
            catMap.Add("Lt", UnicodeCategory.TitlecaseLetter);
            catMap.Add("Lm", UnicodeCategory.ModifierLetter);
            catMap.Add("Lo", UnicodeCategory.OtherLetter);

            catMap.Add("Mn", UnicodeCategory.NonSpacingMark);
            catMap.Add("Mc", UnicodeCategory.SpacingCombiningMark);
            catMap.Add("Me", UnicodeCategory.EnclosingMark);

            catMap.Add("Nd", UnicodeCategory.DecimalDigitNumber);
            catMap.Add("Nl", UnicodeCategory.LetterNumber);
            catMap.Add("No", UnicodeCategory.OtherNumber);

            catMap.Add("Zs", UnicodeCategory.SpaceSeparator);
            catMap.Add("Zl", UnicodeCategory.LineSeparator);
            catMap.Add("Zp", UnicodeCategory.ParagraphSeparator);

            catMap.Add("Cc", UnicodeCategory.Control);
            catMap.Add("Cf", UnicodeCategory.Format);
            catMap.Add("Cs", UnicodeCategory.Surrogate);
            catMap.Add("Co", UnicodeCategory.PrivateUse);

            catMap.Add("Pc", UnicodeCategory.ConnectorPunctuation);
            catMap.Add("Pd", UnicodeCategory.DashPunctuation);
            catMap.Add("Ps", UnicodeCategory.OpenPunctuation);
            catMap.Add("Pe", UnicodeCategory.ClosePunctuation);
            catMap.Add("Pi", UnicodeCategory.InitialQuotePunctuation);
            catMap.Add("Pf", UnicodeCategory.FinalQuotePunctuation);
            catMap.Add("Po", UnicodeCategory.OtherPunctuation);

            catMap.Add("Sm", UnicodeCategory.MathSymbol);
            catMap.Add("Sc", UnicodeCategory.CurrencySymbol);
            catMap.Add("Sk", UnicodeCategory.ModifierSymbol);
            catMap.Add("So", UnicodeCategory.OtherSymbol);
            catMap.Add("Cn", UnicodeCategory.OtherNotAssigned);

            foreach (UnicodeCategory cat in Enum.GetValues(typeof(UnicodeCategory)))
            {
                bool found = false;
                foreach (var kv in catMap)
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

            return catMap;
        }
    }
}
