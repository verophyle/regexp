// Copyright © Verophyle Informatics 2017

using System.Collections.Generic;
using System.Linq;

namespace Verophyle.Regexp.InputSet
{
    public class CharMatcher : InputMatcher<char>
    {
        public CharMatcher(IEnumerable<IInputSet<char>> inputSets)
            : base(inputSets)
        {
        }

        public override IInputSet<char> GetNewSetFromCodes(IEnumerable<int> codes)
        {
            return new CharSet(codes.Select(code => (char)code));
        }
    }
}
