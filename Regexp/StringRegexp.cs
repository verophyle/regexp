// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verophyle.Regexp
{
    public class StringRegexp : DeterministicAutomaton<char>
    {
        string text;
        bool literal;

        public StringRegexp(IEnumerable<char> str, bool literal = false)
            : base()
        {
            this.text = new string(str.ToArray());
            this.literal = literal;

            if (literal)
            {
                var sb = new StringBuilder();
                foreach (var ch in str)
                    sb.AppendFormat("\\U{0:X8}", (int)ch);
                str = sb.ToString();
            }

            var matcher = new RegexpParser();
            var match = matcher.GetMatch(str, matcher.Regex);
            if (match.Success)
            {
                var re = match.Result;
                Compile(re);
            }
            else
            {
                throw new ArgumentException(match.Error);
            }
        }

        public bool Literal { get { return literal; } }

        public override bool Equals(object obj)
        {
            var sr = obj as StringRegexp;
            if (sr != null)
                return text.Equals(sr.text);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return text.GetHashCode();
        }

        public override string ToString()
        {
            return text;
        }
    }
}
