// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using System.Collections.Generic;
using System.Linq;

namespace Verophyle.Regexp
{
    /// <summary>
    /// An input class specialized for characters.
    /// </summary>
    public class CharClass : InputClass<char>
    {
        ISet<int> allInputs = null;

        public override ISet<int> InputCodes
        {
            get { return allInputs ?? (allInputs = GenerateInputCodes()); }
        }

        internal CharClass()
            : base()
        {
        }

        public CharClass(char ch)
            : base(ch)
        {
        }

        public CharClass(IEnumerable<char> str)
            : base(str)
        {
        }

        public CharClass(IEnumerable<InputClass<char>> cls)
            : base(cls)
        {
        }

        public override int GetCode(char input)
        {
            return (int)input;
        }

        public override InputClass<char> GetNewClassFromCodes(IEnumerable<int> codes)
        {
            return new CharClass(codes.Select(code => (char)code));
        }

        protected virtual ISet<int> GenerateInputCodes()
        {
            return new HashSet<int>(Inputs.Select(ch => (int)ch));
        }
    }
}
