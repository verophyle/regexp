// Verophyle.Regexp Copyright © Verophyle Informatics 2017

using System;
using System.Collections.Generic;
using System.Linq;

namespace Verophyle.Regexp.InputSet
{
    /// <summary>
    /// An input class specialized for characters.
    /// </summary>
    public class CharSet : InputSet<char>
    {
        internal CharSet()
            : base()
        {
        }

        public CharSet(char ch)
            : base(ch)
        {
        }

        public CharSet(IEnumerable<char> str)
            : base(str)
        {
        }

        public CharSet(IEnumerable<InputSet<char>> cls)
            : base(cls)
        {
        }

        public override ISet<int> InputCodes
        {
            get { return inputCodes ?? (inputCodes = new HashSet<int>(Inputs.Select(ch => (int)ch))); }
            internal set { inputCodes = value; }
        }
    }
}
