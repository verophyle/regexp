// Verophyle.Regexp Copyright © Verophyle Informatics 2017

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Verophyle.Regexp.InputSet
{
    /// <summary>
    /// A matcher for character sets that returns a regular character class, then unicode category class, then dot if present.
    /// </summary>
    public class UnicodeCategoryMatcher : CharMatcher
    {
        readonly IDictionary<UnicodeCategory, UnicodeCategorySet> categorySets = 
            new Dictionary<UnicodeCategory, UnicodeCategorySet>();

        public UnicodeCategoryMatcher(IEnumerable<IInputSet<char>> inputSets)
            : base(inputSets)
        {
            foreach (var catSet in inputSets.OfType<UnicodeCategorySet>())
            {
                foreach (var uc in catSet.Categories)
                {
                    if (categorySets.ContainsKey(uc))
                        throw new Exception("Unicode category classes must be disjoint.");
                    categorySets[uc] = catSet;
                }
            }
        }

        public override IInputSet<char> GetNewSetFromCodes(IEnumerable<int> codes)
        {
            var result = new UnicodeCategorySet();
            foreach (var code in codes)
            {
                if (code >= 0)
                    result.Inputs.Add((char)code);
                else
                    result.Categories.Add(UnicodeCategorySet.GetCodeCat(code));
            }
            result.InputCodes = new HashSet<int>(codes);
            return result;
        }

        public override IEnumerable<IInputSet<char>> GetSetsThatMatch(char input)
        {
            IInputSet<char> result;
            if (setsByInput.TryGetValue(input, out result))
                yield return result;

            UnicodeCategory uc =
#if NETSTANDARD1_4
                CharUnicodeInfo.GetUnicodeCategory(input);
#else
                char.GetUnicodeCategory(input);
#endif

            UnicodeCategorySet cc;
            if (categorySets.TryGetValue(uc, out cc))
                yield return cc;

            if (dotClass != null)
                yield return dotClass;
        }
    }
}
