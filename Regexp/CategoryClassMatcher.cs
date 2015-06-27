// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Verophyle.Regexp
{
    /// <summary>
    /// A matcher for CharClasses that returns a regular character class, then unicode category class, then dot if present.
    /// </summary>
    public class CategoryClassMatcher : SimpleSetMatcher<char>
    {
        IDictionary<UnicodeCategory, CategoryClass> category_classes = new Dictionary<UnicodeCategory, CategoryClass>();

        public CategoryClassMatcher(IEnumerable<InputClass<char>> input_classes)
            : base(input_classes)
        {
            foreach (var input_class in input_classes.OfType<CategoryClass>())
            {
                foreach (var uc in input_class.Categories)
                {
                    if (category_classes.ContainsKey(uc))
                        throw new Exception("Unicode category classes must be disjoint.");
                    category_classes[uc] = input_class;
                }
            }
        }

        public override IEnumerable<InputClass<char>> ClassThatMatched(char input)
        {
            // we can't call the base class's function in an iterator block, due to a 
            // bug in the compiler, so we replicate the base class function

            InputClass<char> result;
            if (inputs_to_class.TryGetValue(input, out result))
                yield return result;

            CategoryClass cc;
            if (category_classes.TryGetValue(char.GetUnicodeCategory(input), out cc))
                yield return cc;

            if (dot_class != null)
                yield return dot_class;
        }
    }
}
