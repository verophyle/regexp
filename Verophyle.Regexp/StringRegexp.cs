// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verophyle.Regexp
{
    public class StringRegexp : DeterministicAutomaton<char, InputSet.UnicodeCategoryMatcher>
    {
        string text;
        bool literal;

        public StringRegexp(IEnumerable<char> spec, bool literal = false)
            : base()
        {
            this.text = new string(spec.ToArray());
            this.literal = literal;

            if (literal)
            {
                var sb = new StringBuilder();
                foreach (var ch in spec)
                    sb.AppendFormat("\\U{{{0:X8}}}", (int)ch);
                spec = sb.ToString();
            }

            var errors = new List<string>();
            int start = 0, next;
            var node = StringRegexpParser.ParseRegexp(spec, start, out next, errors);
            if (node != null && !errors.Any())
            {
                Compile(node);
            }
            else
            {
                var msg = errors.Any()
                    ? string.Join(", ", errors)
                    : "unexpected error";
                throw new ArgumentException(msg, nameof(spec));
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
